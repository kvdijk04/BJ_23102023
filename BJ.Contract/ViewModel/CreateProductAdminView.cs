using BJ.Contract.Product;
using Microsoft.AspNetCore.Http;

namespace BJ.Contract.ViewModel
{
    public class CreateProductAdminView
    {
        public CreateProductDto CreateProduct { get; set; } = new CreateProductDto();
        public int[] Size { get; set; }

        public int[] SubCat { get; set; }
        public IFormFile ImageCup { get; set; }
        public IFormFile ImageHero { get; set; }

        public IFormFile ImageIngredients { get; set; }


    }
}
