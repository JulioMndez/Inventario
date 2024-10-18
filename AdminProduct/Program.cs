using System;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Crud;
public struct Producto
{
    public int Id;
    public string Nombre;
    public int Cantidad;
    public decimal Precio;
    public Producto(int id, string nombre, int cantidad, decimal precio)
    {
        Id = id;
        Nombre = nombre;
        Cantidad = cantidad;
        Precio = precio;
    }
}
public class Crud
{
    private const string archivo = "productos.dat";
    public static void Main()
    {
        int opcion;
        do
        {
            Console.WriteLine("\n--- Menú CRUD ---");
            Console.WriteLine("1. Crear Producto");
            Console.WriteLine("2. Leer Productos");
            Console.WriteLine("3. Actualizar Producto");
            Console.WriteLine("4. Calcular Valor Total del Inventario");
            Console.WriteLine("5. Salir");
            Console.Write("Seleccione una opción: ");
            opcion = int.Parse(Console.ReadLine());

            switch (opcion)
            {
                case 1: AgregarProducto(); break;
                case 2: LeerProductos(); break;
                case 3: ActualizarProducto(); break;
                case 4: valorInventario(); break;
                case 5: break;
                default: break;
            }
        } while (opcion != 5);
    }
    public static void AgregarProducto()
    {
        Console.Write("Ingrese el ID del producto: ");
        int id = int.Parse(Console.ReadLine());
        Console.Write("Ingrese el nombre del producto: ");
        string nombre = Console.ReadLine();
        Console.Write("Ingrese la cantidad de productos: ");
        int cantidad = int.Parse(Console.ReadLine());
        Console.Write("Ingrese el precio del producto: ");
        decimal precio = decimal.Parse(Console.ReadLine());

        Producto producto = new Producto(id, nombre, cantidad, precio);

        using (BinaryWriter writer = new BinaryWriter(File.Open(archivo, FileMode.Append)))
        {
            writer.Write(producto.Id);
            writer.Write(producto.Nombre);
            writer.Write(producto.Cantidad);
            writer.Write(producto.Precio);
        }
        Console.WriteLine("Producto agregado exitosamente.");
    }
    public static void LeerProductos()
    {
        if (!File.Exists(archivo))
        {
            Console.WriteLine("No hay productos para mostrar.");
            return;
        }

        using (BinaryReader reader = new BinaryReader(File.Open(archivo, FileMode.Open)))
        {
            Console.WriteLine("\nProductos");
            try
            {
                while (true)
                {
                    Producto producto = new Producto()
                    {
                        Id = reader.ReadInt32(),
                        Nombre = reader.ReadString(),
                        Cantidad = reader.ReadInt32(),
                        Precio = reader.ReadDecimal()
                    };
                    Console.WriteLine($"ID: {producto.Id}");
                    Console.WriteLine($"Nombre: {producto.Nombre}");
                    Console.WriteLine($"Cantidad: {producto.Cantidad}");
                    Console.WriteLine($"Precio: {producto.Precio}");
                    Console.WriteLine();
                }
            }
            catch (EndOfStreamException) { }
        }
    }
    public static void ActualizarProducto()
    {
        Console.Write("Ingrese el ID del producto: ");
        int idBuscado = int.Parse(Console.ReadLine());
        bool encontrado = false;
        string archivo_temp = "temporal.dat";

        using (BinaryReader reader = new BinaryReader(File.Open(archivo, FileMode.Open)))
        using (BinaryWriter writer = new BinaryWriter(File.Open(archivo_temp, FileMode.Create)))
        {
            try
            {
                while (true)
                {

                    Producto producto = new Producto()
                    {
                        Id = reader.ReadInt32(),
                        Nombre = reader.ReadString(),
                        Cantidad = reader.ReadInt32(),
                        Precio = reader.ReadDecimal()
                    };

                    if (producto.Id == idBuscado)
                    {
                        Console.WriteLine("Ingrese el nuevo nombre: ");
                        producto.Nombre = Console.ReadLine();
                        Console.Write("Ingrese la nueva cantidad: ");
                        producto.Cantidad = int.Parse(Console.ReadLine());
                        Console.WriteLine("Ingrese el nuevo precio: ");
                        producto.Precio = int.Parse(Console.ReadLine());

                        encontrado = true;
                    }
                    writer.Write(producto.Id);
                    writer.Write(producto.Nombre);
                    writer.Write(producto.Cantidad);
                    writer.Write(producto.Precio);
                }

            }

            catch (EndOfStreamException) { }

        }

        File.Delete(archivo);
        File.Move(archivo_temp, archivo);

        if (encontrado)
            Console.WriteLine("El producto fue actualizado correctamente...");
        else
            Console.WriteLine("El producto no se encontró...");

    }
    public static void valorInventario()
    {
        if (!File.Exists(archivo))
        {
            Console.WriteLine("No hay productos para calcular el inventario.");
            return;
        }

        decimal totalInventario = 0;

        using (BinaryReader reader = new BinaryReader(File.Open(archivo, FileMode.Open)))
        {
            try
            {
                while (true)
                {
                    Producto producto = new Producto()
                    {
                        Id = reader.ReadInt32(),
                        Nombre = reader.ReadString(),
                        Cantidad = reader.ReadInt32(),
                        Precio = reader.ReadDecimal()
                    };

                    // Calcular el valor total del producto
                    decimal valorProducto = producto.Cantidad * producto.Precio;
                    totalInventario += valorProducto;
                }
            }
            catch (EndOfStreamException) { }
        }

        Console.WriteLine($"El valor total del inventario es: {totalInventario:C}");
    }

}


