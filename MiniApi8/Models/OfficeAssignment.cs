﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;

namespace MiniApi8.Models;

public partial class OfficeAssignment
{
    public int InstructorId { get; set; }

    public string? Location { get; set; }

    public virtual Person Instructor { get; set; } = null!;
}