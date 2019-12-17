﻿using System;
using System.Collections.Generic;

namespace School.Data
{
    public partial class ClassSubject
    {
        public long ClassId { get; set; }
        public long SubjectId { get; set; }

        public virtual Class Class { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
