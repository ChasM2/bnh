﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel;
using System.Collections;
using Ms.Cms.Models;

using TypeMap = System.Collections.Generic.Dictionary<string, System.Type>;
using Ms.Cms.Controllers;

namespace Ms.Cms
{
    public class BnhModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            bool hasPrefix = bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName);
            string prefix = ((hasPrefix) && (bindingContext.ModelName != "")) ? bindingContext.ModelName + "." : "";

            if (controllerContext.Controller.GetType() == typeof(SceneController))
            {
                if (modelType.IsAssignableFrom(typeof(Brick)))
                {
                    var model = new DesignBrick();
                    bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, model.GetType());
                    return model;
                }

                if (modelType.IsAssignableFrom(typeof(Wall)))
                {
                    var model = new DesignWall();
                    bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, model.GetType());
                    return model;
                }
            }

            return base.CreateModel(controllerContext, bindingContext, modelType);           
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

                        // TODO: check it used to be typeof(EntityCollection<>)
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
