﻿using System.Runtime.Serialization;

namespace Immense.RemoteControl.Shared.Models.Dtos
{
    [DataContract]
    public class MouseDownDto : BaseDto
    {
        [DataMember(Name = "Button")]
        public int Button { get; set; }

        [DataMember(Name = "DtoType")]
        public override DtoType DtoType { get; init; } = DtoType.MouseDown;

        [DataMember(Name = "PercentX")]
        public double PercentX { get; set; }

        [DataMember(Name = "PercentY")]
        public double PercentY { get; set; }
    }
}
