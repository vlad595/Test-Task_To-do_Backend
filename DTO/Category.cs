using System;
using Models;

namespace DTO
{
    public class CategoryCreationDto
    {
        public string color {get;set;}
        public string name { get; set; }
    }
    public class CategoryResponseDto
    {
        public int id { get; set; }
        public string color {get;set;}
        public string name { get; set; }
    }
    public class CategoryItemResponseDto
    {
        public int Id {get;set;}
        public string Color {get;set;}
        public string Name {get;set;}
        public List<ToDoTask> Tasks {get;set;}
    }
}