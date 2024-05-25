using bookDemo.Controllers;
using bookDemo.Models;

namespace bookDemo.Data
{
    public static class ApplicationContext
    {
        public static List<Book> Books { get; set; }
        static ApplicationContext()
        {
            Books =
            [
                new() {Id=1, Title="Karegöz ve Hacivat", Price=75},
                new() {Id=2, Title="Mesnevi", Price=150},
                new() {Id=3, Title="Dede Korkut", Price=50}
            ];
        }
    }
}
