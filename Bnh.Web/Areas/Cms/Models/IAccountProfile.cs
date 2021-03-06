﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cms.Models
{
    public interface IAccountProfile
    {
        string UserName { get; }

        string DisplayName { get; }

        string RealName { get; }

        string GravatarEmail { get; }

        DateTime LastUpdatedDate { get; }

        DateTime LastActivityDate { get; }
    }
}
