﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel;
using System.Collections;

using TypeMap = System.Collections.Generic.Dictionary<string, System.Type>;
using Cms.Controllers;
using Cms.Models;
using Cms.ViewModels;

namespace Cms.Helpers
{
    public class BnhModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            bool hasPrefix = bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName);
            string prefix = ((hasPrefix) && (bindingContext.ModelName != "")) ? bindingContext.ModelName + "." : "";

            if (typeof(SceneController).IsAssignableFrom(controllerContext.Controller.GetType()))
            {
                if (modelType.IsAssignableFrom(typeof(IBrickViewModel<>)))
                {
                    // get the parameter species
                    var result = bindingContext.ValueProvider.GetValue(prefix + "BrickType");
                    if (result == null || string.IsNullOrEmpty(result.AttemptedValue))
                    {
                        throw new Exception(string.Format("Unknown type \"{0}\"", result.AttemptedValue));
                    }

                    var type = MsCms.RegisteredBrickTypes.Select(br => br.Type).First(t => t.Name == result.AttemptedValue);
                    var model = BrickViewModel<Brick>.Create(type);
                    bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, model.GetType());
                    return model;
                }
            }

            try
            {
                return base.CreateModel(controllerContext, bindingContext, modelType);
            }
            catch(Exception)
            {
                throw;
            }
        }        

        protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
        {
            ModelMetadata propertyMetadata = bindingContext.PropertyMetadata[propertyDescriptor.Name];
            propertyMetadata.Model = value;
            string modelStateKey = CreateSubPropertyName(bindingContext.ModelName, propertyMetadata.PropertyName);

            // Try to set a value into the property unless we know it will fail (read-only 
            // properties and null values with non-nullable types)
            if (!propertyDescriptor.IsReadOnly)
            {
                try
                {
                    if (value == null)
                    {
                        propertyDescriptor.SetValue(bindingContext.Model, value);
                    }
                    else
                    {
                        Type valueType = value.GetType();

                        if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                        {
                            IListSource ls = (IListSource)propertyDescriptor.GetValue(bindingContext.Model);
                            IList list = ls.GetList();

                            foreach (var item in (IEnumerable)value)
                            {
                                list.Add(item);
                            }
                        }
                        else
                        {
                            propertyDescriptor.SetValue(bindingContext.Model, value);
                        }
                    }

                }
                catch (Exception ex)
                {
                    // Only add if we're not already invalid
                    if (bindingContext.ModelState.IsValidField(modelStateKey))
                    {
                        bindingContext.ModelState.AddModelError(modelStateKey, ex);
                    }
                }
            }
        }
    }
}
