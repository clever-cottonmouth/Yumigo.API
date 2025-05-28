using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yumigo.API.Models;

namespace Yumigo.API.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<MenuItem> menuItems { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem { Id = 1, Name = "Garlic Bread", Description = "Toasted bread with garlic butter and herbs", Image = "garlic_bread.jpg", Price = 5.99, Category = "Appetizer", SpecialTag = "Vegetarian" },
                 new MenuItem { Id = 2, Name = "Chicken Wings", Description = "Spicy buffalo wings with blue cheese dip", Image = "chicken_wings.jpg", Price = 12.99, Category = "Appetizer", SpecialTag = "Spicy" },
                 new MenuItem { Id = 3, Name = "Caesar Salad", Description = "Crisp romaine with Caesar dressing and croutons", Image = "caesar_salad.jpg", Price = 8.49, Category = "Salad", SpecialTag = "Vegetarian" },
                 new MenuItem { Id = 4, Name = "Margherita Pizza", Description = "Classic pizza with tomato, mozzarella, and basil", Image = "margherita_pizza.jpg", Price = 14.99, Category = "Main Course", SpecialTag = "Vegetarian" },
                 new MenuItem { Id = 5, Name = "Grilled Salmon", Description = "Fresh salmon with lemon herb sauce", Image = "grilled_salmon.jpg", Price = 22.99, Category = "Main Course", SpecialTag = "Gluten-Free" },
                 new MenuItem { Id = 6, Name = "Beef Burger", Description = "Juicy beef patty with lettuce, tomato, and cheese", Image = "beef_burger.jpg", Price = 13.99, Category = "Main Course", SpecialTag = "" },
                 new MenuItem { Id = 7, Name = "Mushroom Risotto", Description = "Creamy risotto with wild mushrooms", Image = "mushroom_risotto.jpg", Price = 16.49, Category = "Main Course", SpecialTag = "Vegetarian" },
                 new MenuItem { Id = 8, Name = "French Fries", Description = "Crispy golden fries with ketchup", Image = "french_fries.jpg", Price = 4.99, Category = "Side", SpecialTag = "Vegetarian" },
                 new MenuItem { Id = 9, Name = "Onion Rings", Description = "Crispy battered onion rings", Image = "onion_rings.jpg", Price = 6.49, Category = "Side", SpecialTag = "Vegetarian" },
                 new MenuItem { Id = 10, Name = "Chocolate Lava Cake", Description = "Warm chocolate cake with molten center", Image = "lava_cake.jpg", Price = 7.99, Category = "Dessert", SpecialTag = "Indulgent" },
                 new MenuItem { Id = 11, Name = "Cheesecake", Description = "Creamy cheesecake with berry compote", Image = "cheesecake.jpg", Price = 8.99, Category = "Dessert", SpecialTag = "" },
                 new MenuItem { Id = 12, Name = "Mojito", Description = "Refreshing mint and lime cocktail", Image = "mojito.jpg", Price = 9.99, Category = "Beverage", SpecialTag = "Alcoholic" },
                 new MenuItem { Id = 13, Name = "Iced Coffee", Description = "Chilled coffee with milk and sugar", Image = "iced_coffee.jpg", Price = 4.49, Category = "Beverage", SpecialTag = "" },
                 new MenuItem { Id = 14, Name = "Veggie Wrap", Description = "Whole wheat wrap with fresh veggies and hummus", Image = "veggie_wrap.jpg", Price = 10.99, Category = "Main Course", SpecialTag = "Vegan" },
                 new MenuItem { Id = 15, Name = "Shrimp Cocktail", Description = "Chilled shrimp with tangy cocktail sauce", Image = "shrimp_cocktail.jpg", Price = 11.99, Category = "Appetizer", SpecialTag = "Gluten-Free" },
                 new MenuItem { Id = 16, Name = "Pasta Alfredo", Description = "Fettuccine in creamy Alfredo sauce", Image = "pasta_alfredo.jpg", Price = 15.49, Category = "Main Course", SpecialTag = "Vegetarian" },
                 new MenuItem { Id = 17, Name = "Greek Salad", Description = "Feta, olives, and cucumbers in olive oil dressing", Image = "greek_salad.jpg", Price = 9.49, Category = "Salad", SpecialTag = "Vegetarian" },
                 new MenuItem { Id = 18, Name = "BBQ Ribs", Description = "Slow-cooked ribs with smoky BBQ sauce", Image = "bbq_ribs.jpg", Price = 24.99, Category = "Main Course", SpecialTag = "Spicy" },
                 new MenuItem { Id = 19, Name = "Lemonade", Description = "Freshly squeezed lemonade with mint", Image = "lemonade.jpg", Price = 3.99, Category = "Beverage", SpecialTag = "Non-Alcoholic" },
                 new MenuItem { Id = 20, Name = "Tiramisu", Description = "Classic Italian dessert with coffee and mascarpone", Image = "tiramisu.jpg", Price = 8.49, Category = "Dessert", SpecialTag = "" }
            );
        }

    }
}
