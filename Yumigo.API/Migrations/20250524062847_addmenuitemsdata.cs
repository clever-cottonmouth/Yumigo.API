using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Yumigo.API.Migrations
{
    /// <inheritdoc />
    public partial class addmenuitemsdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "menuItems",
                columns: new[] { "Id", "Category", "Description", "Image", "Name", "Price", "SpecialTag" },
                values: new object[,]
                {
                    { 1, "Appetizer", "Toasted bread with garlic butter and herbs", "garlic_bread.jpg", "Garlic Bread", 5.9900000000000002, "Vegetarian" },
                    { 2, "Appetizer", "Spicy buffalo wings with blue cheese dip", "chicken_wings.jpg", "Chicken Wings", 12.99, "Spicy" },
                    { 3, "Salad", "Crisp romaine with Caesar dressing and croutons", "caesar_salad.jpg", "Caesar Salad", 8.4900000000000002, "Vegetarian" },
                    { 4, "Main Course", "Classic pizza with tomato, mozzarella, and basil", "margherita_pizza.jpg", "Margherita Pizza", 14.99, "Vegetarian" },
                    { 5, "Main Course", "Fresh salmon with lemon herb sauce", "grilled_salmon.jpg", "Grilled Salmon", 22.989999999999998, "Gluten-Free" },
                    { 6, "Main Course", "Juicy beef patty with lettuce, tomato, and cheese", "beef_burger.jpg", "Beef Burger", 13.99, "" },
                    { 7, "Main Course", "Creamy risotto with wild mushrooms", "mushroom_risotto.jpg", "Mushroom Risotto", 16.489999999999998, "Vegetarian" },
                    { 8, "Side", "Crispy golden fries with ketchup", "french_fries.jpg", "French Fries", 4.9900000000000002, "Vegetarian" },
                    { 9, "Side", "Crispy battered onion rings", "onion_rings.jpg", "Onion Rings", 6.4900000000000002, "Vegetarian" },
                    { 10, "Dessert", "Warm chocolate cake with molten center", "lava_cake.jpg", "Chocolate Lava Cake", 7.9900000000000002, "Indulgent" },
                    { 11, "Dessert", "Creamy cheesecake with berry compote", "cheesecake.jpg", "Cheesecake", 8.9900000000000002, "" },
                    { 12, "Beverage", "Refreshing mint and lime cocktail", "mojito.jpg", "Mojito", 9.9900000000000002, "Alcoholic" },
                    { 13, "Beverage", "Chilled coffee with milk and sugar", "iced_coffee.jpg", "Iced Coffee", 4.4900000000000002, "" },
                    { 14, "Main Course", "Whole wheat wrap with fresh veggies and hummus", "veggie_wrap.jpg", "Veggie Wrap", 10.99, "Vegan" },
                    { 15, "Appetizer", "Chilled shrimp with tangy cocktail sauce", "shrimp_cocktail.jpg", "Shrimp Cocktail", 11.99, "Gluten-Free" },
                    { 16, "Main Course", "Fettuccine in creamy Alfredo sauce", "pasta_alfredo.jpg", "Pasta Alfredo", 15.49, "Vegetarian" },
                    { 17, "Salad", "Feta, olives, and cucumbers in olive oil dressing", "greek_salad.jpg", "Greek Salad", 9.4900000000000002, "Vegetarian" },
                    { 18, "Main Course", "Slow-cooked ribs with smoky BBQ sauce", "bbq_ribs.jpg", "BBQ Ribs", 24.989999999999998, "Spicy" },
                    { 19, "Beverage", "Freshly squeezed lemonade with mint", "lemonade.jpg", "Lemonade", 3.9900000000000002, "Non-Alcoholic" },
                    { 20, "Dessert", "Classic Italian dessert with coffee and mascarpone", "tiramisu.jpg", "Tiramisu", 8.4900000000000002, "" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "menuItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "menuItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "menuItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "menuItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "menuItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "menuItems",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "menuItems",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "menuItems",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "menuItems",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "menuItems",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "menuItems",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "menuItems",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "menuItems",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "menuItems",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "menuItems",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "menuItems",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "menuItems",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "menuItems",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "menuItems",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "menuItems",
                keyColumn: "Id",
                keyValue: 20);
        }
    }
}
