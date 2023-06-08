﻿using FinalProject.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Areas.AdminArea.ViewModels.SongCRUD
{
    public class SongCreateVM
    {
        [Required(ErrorMessage = "Should be not empty")]
        public IFormFile Audio{ get; set; } = null!;
        public string Name { get; set; } = null!;
        public int AlbumId { get; set; }
        public Album Album { get; set; } = null!;
    }
}