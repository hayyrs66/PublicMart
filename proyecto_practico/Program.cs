using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Supermercado
{
    class Producto
    {
        // Propiedades de la clase Producto
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }

        // Constructor de la clase Producto
        public Producto(string codigo, string nombre, double precio)
        {
            Codigo = codigo;
            Nombre = nombre;
            Precio = precio;
        }
    }

    class Factura
    {
        // Propiedades de la clase Factura
        public string Nit { get; set; }
        public string CorreoElectronico { get; set; }
        public string NombreCliente { get; set; }
        public Dictionary<Producto, int> Productos { get; set; }
        public double Subtotal { get; set; }
        public double ImpuestoISR { get; set; }
        public double ImpuestoIVA { get; set; }
        public double Total { get; set; }

        public string MetodoPago { get; set; }
        public int Puntos { get; set; }

        public string Fecha { get; set; }
        public int Numero { get; set; }

        public int CantidadProductos { get; set; }

        // Constructor de la clase Factura
        public Factura(string nit, string correoElectronico, string nombreCliente)
        {
            Nit = nit;
            CorreoElectronico = correoElectronico;
            NombreCliente = nombreCliente;
            Productos = new Dictionary<Producto, int>();
            Fecha = DateTime.Now.ToString("dd/MM/yyyy");
            Numero = GenerarNumeroFactura();
        }
        // Método para agregar un producto a la factura
        public void AgregarProducto(Producto producto, int cantidad)
        {
            if (Productos.ContainsKey(producto))
            {
                Productos[producto] += cantidad;
            }
            else
            {
                Productos.Add(producto, cantidad);
            }
            CantidadProductos += cantidad;
            Subtotal += producto.Precio * cantidad;
            ImpuestoISR = Subtotal * 0.05;
            ImpuestoIVA = Subtotal * 0.12;
            Total = Subtotal + ImpuestoIVA + ImpuestoISR;
        }

        // Método para generar un número de factura
        private int GenerarNumeroFactura()
        {
            Random rand = new Random();
            int x = rand.Next(1, 1000);
            int p = Puntos;
            int n = CantidadProductos;
            int numero = ((2 * n + p * p) + (2021 * n)) % 10000;
            return numero;
        }

        // Método para imprimir la factura
        public string Imprimir()
        {
            Console.Clear();
            string factura = "";
            factura += "Nombre del supermercado: PublicMart\n";
            factura += $"Fecha de la factura {Fecha}\n";
            factura += $"Numero de factura: {Numero.ToString("D4")}\n";
            factura += $"NIT: {Nit}\n";
            factura += $"Nombre del cliente: {NombreCliente}\n";
            factura += "Productos:\n";

            foreach (KeyValuePair<Producto, int> kvp in Productos)
            {
                Producto producto = kvp.Key;
                int cantidad = kvp.Value;
                double precioUnitario = producto.Precio;
                double precioTotal = precioUnitario * cantidad;
                factura += $"- {producto.Nombre}: {cantidad} " +
                    $"unidades x {precioUnitario:C2} = {precioTotal:C2}\n";
            }

            factura += $"Cantidad de productos adquiridos: {CantidadProductos} \n";
            factura += $"Subtotal: {Subtotal:C2} \n";
            factura += $"Impuesto ISR: {ImpuestoISR:C2} \n";
            factura += $"Impuesto IVA: {ImpuestoIVA:C2} \n";
            factura += $"Total de la factura: {Total:C2} \n";
            factura += $"Metodo de pago: {(MetodoPago == "1" ? "Efectivo" : "Tarjeta de Credito")}\n";
            factura += $"Total puntos acumulados: {Puntos}\n";
            factura += $"Una copia de la factura se enviará al correo: {CorreoElectronico} \n";
            return factura;
        }
    }

    class Program
    {
        // Listas que almacenan las facturas y los productos
        static List<Factura> facturas = new List<Factura>();
        static List<Producto> productos = new List<Producto>();

        static void Main(string[] args)
        {
            // Agregar productos a la lista de productos
            productos.Add(new Producto("001", "Pan Frances", 1.10));
            productos.Add(new Producto("002", "Libra de azucar", 5.00));
            productos.Add(new Producto("003", "Caja de galletas", 7.30));
            productos.Add(new Producto("004", "Caja de granola", 32.50));
            productos.Add(new Producto("005", "Litro de jugo de naranja", 17.95));

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Bienvenido al sistema de facturizacion de PublicMart");
                Console.WriteLine("Seleccione una opcion");
                Console.WriteLine("1. Facturacion");
                Console.WriteLine("2. Reportes de facturacion");
                Console.WriteLine("3. Cerrar programa");

                string opcion = Console.ReadLine();

                // Estructura switch para manejar las opciones del menú
                switch (opcion)
                {
                    case "1":
                        Facturacion();
                        break;

                    case "2":
                        Reportes();
                        break;

                    case "3":
                        return;
                    default:
                        Console.WriteLine("Opcion invalida, presione cualquier tecla para continuar");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Método para validar si un correo electrónico es válido
        private static bool EsCorreoValido(string correo)
        {
            string regularExpression = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
        @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
         @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            Regex regex = new Regex(regularExpression);
            return regex.IsMatch(correo);

        }

        // Método para manejar el proceso de facturación
        static void Facturacion()
        {
            // Ingrese y valide datos del cliente
            Console.Clear();
            Console.WriteLine("Ingrese el NIT del cliente");
            string nit = Console.ReadLine();

            // Validar que el NIT no este vacio
            while (string.IsNullOrWhiteSpace(nit))
            {
                Console.WriteLine("NIT Invalido, por favor ingrese un NIT");
                nit = Console.ReadLine();
            }

            Console.WriteLine("Ingrese el correo electronico del cliente:");
            string correoElectronico = Console.ReadLine();

            // Usar la funcion EsCorreoValido para validar correo
            while (!EsCorreoValido(correoElectronico))
            {
                Console.WriteLine("Correo electronico invalido, ingrese un correo valido");
                correoElectronico = Console.ReadLine();
            }

            Console.WriteLine("Ingrese el nombre del cliente:");
            string nombre = Console.ReadLine();

            // Validar que el Nombre no este vacio
            while (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("Nombre Invalido, por favor ingrese un nombre valido");
                nombre = Console.ReadLine();
            }

            // Crear un objeto factura con los datos del cliente
            Factura factura = new Factura(nit, correoElectronico, nombre);

            // Bucle para agregar productos a la factura
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Ingrese el codigo del producto");
                string codigo = Console.ReadLine();

                // Buscar el producto por su código
                Producto producto = productos.Find(p => p.Codigo == codigo);

                if (producto == null)
                {
                    Console.WriteLine("Codigo de producto invalido, presione cualquier tecla para continuar");
                    Console.ReadKey();
                    continue;
                }

                // Ingrese la cantidad de productos que el cliente desea comprar
                Console.WriteLine($"Ingrese la cantidad de {producto.Nombre} que lleva el cliente");
                string cantidadString = Console.ReadLine();

                if (!int.TryParse(cantidadString, out int cantidad) || cantidad <= 0)
                {
                    Console.WriteLine("Cantidad invalida, presione cualquier tecla para continuar");
                    Console.ReadKey();
                    continue;
                }
                // Agregar producto y cantidad a la factura
                factura.AgregarProducto(producto, cantidad);

                Console.WriteLine("Producto agregado a la factura");
                Console.WriteLine("Desea agregar otro producto? (s/n)");
                string opcion = Console.ReadLine().ToLower();

                if (opcion != "s")
                {
                    break;
                }
            }

            // Seleccione y valide el método de pago
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Seleccione un metodo de pago");
                Console.WriteLine("1. Efectivo");
                Console.WriteLine("2. Tarjeta de credito");
                string metodoPago = Console.ReadLine();

                if (metodoPago != "1" && metodoPago != "2")
                {
                    Console.WriteLine("Metodo de pago invalido, presione cualquier tecla para continuar");
                    Console.ReadKey();
                    continue;
                }

                // Asignar el método de pago y calcular los puntos si se paga con tarjeta de crédito
                factura.MetodoPago = metodoPago;

                if (metodoPago == "2")
                {
                    if (factura.Total < 50)
                    {
                        factura.Puntos = (int)Math.Floor(factura.Total / 10);
                    }
                    else if (factura.Total >= 50 && factura.Total < 150)
                    {
                        factura.Puntos = (int)Math.Floor(factura.Total / 10) * 2;
                    }
                    else if (factura.Total >= 150)
                    {
                        factura.Puntos = (int)Math.Floor(factura.Total / 15) * 3;
                    }
                }
                // Agregar la factura a la lista de facturas
                facturas.Add(factura);

                // Imprimir la factura y esperar la entrada del usuario para continuar
                Console.WriteLine(factura.Imprimir());
                Console.WriteLine("Presione cualquier tecla para continuar");
                Console.ReadKey();
                break;
            }
        }

        // Método para mostrar reportes de facturación
        static void Reportes()
        {
            Console.Clear();
            Console.WriteLine("Reportes de facturacion:\n");
            Console.WriteLine($"Total facturas realizadas: {facturas.Count}");
            int totalProductos = 0;
            double totalVentas = 0;
            Dictionary<Producto, int> detalleProductos = new Dictionary<Producto, int>();

            foreach (Factura factura in facturas)
            {
                totalProductos += factura.CantidadProductos;
                totalVentas += factura.Total;

                // Iterar a través de todas las facturas y calcular el total de productos vendidos,
                // el total de ventas y los detalles de ventas de cada producto
                foreach (KeyValuePair<Producto, int> kvp in factura.Productos)
                {
                    Producto producto = kvp.Key;
                    int cantidad = kvp.Value;

                    if (detalleProductos.ContainsKey(producto))
                    {
                        detalleProductos[producto] += cantidad;

                    }
                    else
                    {
                        detalleProductos.Add(producto, cantidad);
                    }
                }
            }

            Console.WriteLine("Detalle por producto:");

            foreach (KeyValuePair<Producto, int> kvp in detalleProductos)
            {
                Producto producto = kvp.Key;
                int cantidad = kvp.Value;

                Console.WriteLine($"{producto.Nombre}:{cantidad} unidades");
            }

            // Calcular y mostrar el total de puntos generados, las ventas al crédito,
            // las ventas al contado y el total de ventas
            int totalPuntos = 0;
            double totalVentasEfectivo = 0;
            double totalVentasTarjeta = 0;

            foreach (Factura factura in facturas)
            {
                totalPuntos += factura.Puntos;

                if (factura.MetodoPago == "1")
                {
                    totalVentasEfectivo += factura.Total;

                }
                else if (factura.MetodoPago == "2")
                {
                    totalVentasTarjeta += factura.Total;
                }
            }
            Console.WriteLine($"Total de puntos generados: {totalPuntos}");
            Console.WriteLine($"Total de ventas al credito {totalVentasTarjeta:C2}");
            Console.WriteLine($"Total de ventas al contado: {totalVentasEfectivo:C2}");
            Console.WriteLine($"Total de ventas: {totalVentas:C2}");
            Console.WriteLine("Presione cualquier tecla para continuar");
            Console.ReadKey();

        }
    }
}