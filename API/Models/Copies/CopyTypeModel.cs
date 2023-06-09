﻿namespace API.Models
{
    public class CopyTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int PlatformId { get; set; }
        public PlatformModel? Platform { get; set; }
        public List<RegionModel>? AvailableRegions { get; set; }
    }
}