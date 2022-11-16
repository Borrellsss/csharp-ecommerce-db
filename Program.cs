//Prima parte
//Dato lo schema in allegato, producete tutte le classi e le relazioni necessarie per poter utilizzare EntityFramework
//(in modalità code-first) al fine di generare il relativo database.
//Seconda Parte
//Considerando che:
//ci sono clienti che effettuano ordini.
//Un ordine viene preparato da un dipendente.
//Un ordine ha associato uno o più pagamenti (considerando eventuali tentativi falliti)
//Realizzate le seguenti funzionalità
//inserite 10 prodotti all’avvio del programma (i prodotti non devono essere inseriti in caso si riavvi l’applicazione)
//quando l’applicazione si avvia chiede se l’utente è un dipendete o un cliente
//se è un dipendente potrà eseguire CRUD sugli ordini
//se è un cliente potrà acquistare degli ordini
//simulate randomicamente l’esito di un acquisto (status = ok | status = ko)
//Bonus
//Il dipendente deve poter spedire gli ordini acquistati per cui il pagamento è andato a buon fine. 

using Microsoft.EntityFrameworkCore;

EcommerceDbContext db = new EcommerceDbContext();

List<Product> products = db.Products.ToList();

if(products.Count() < 10)
{
    for (int i = 0; i < 10; i++)
    {
        Product product = new Product();
        product.Name = $"Prodotto{i}";
        product.Description = $"Incredibile nuovo Prodotto{i}";
        product.Price = new Random().Next(10, 20 + 1);
        db.Products.Add(product);
        db.SaveChanges();
    }
}

Customer customer = new Customer();
customer.Name = "Mario";
customer.Surname = "Rossi";
customer.Email = "mario_rossi@mail.it";

List<Customer> customers = db.Customers.ToList();

bool checkCustomerResult = false;
foreach(Customer _customer in customers)
{
    if(_customer.Email == customer.Email)
    {
        checkCustomerResult = true;
    }
}

if(!checkCustomerResult)
{
    db.Customers.Add(customer);
    db.SaveChanges();
}

Employee employee = new Employee();
employee.Name = "Francesca";
employee.Surname = "Verdi";
employee.Level = "base";

List<Employee> employees = db.Employees.ToList();

bool checkEmployeeResult = false;
foreach (Employee _employee in employees)
{
    if (_employee.Name == employee.Name && _employee.Surname == employee.Surname && _employee.Level == employee.Level)
    {
        checkEmployeeResult = true;
    }
}

if (!checkEmployeeResult)
{
    db.Employees.Add(employee);
    db.SaveChanges();
}

bool runProgram = true;
bool emplyeeRequest = true;
bool customerRequest = true;
while (runProgram)
{
    bool customerOrEmplyeeRequest = true;
    while (customerOrEmplyeeRequest)
    {
        Console.WriteLine("Sei un cliente o un dipendente?");
        Console.WriteLine("[1]: cliente.");
        Console.WriteLine("[2]: dipendente.");
        Console.WriteLine("[3]: termina programma.");

        string customerOrEmplyeeResponse = Console.ReadLine();
        switch (customerOrEmplyeeResponse)
        {
            case "1":
                customerOrEmplyeeRequest = false;
                CustomerActions();
                StopProgram();
                break;

            case "2":
                customerOrEmplyeeRequest = false;
                EmployeeActions();
                StopProgram();
                break;

            case "3":
                Console.WriteLine("Grazie e arrivederci!");
                customerOrEmplyeeRequest = false;
                runProgram = false;
                break;

            default:
                Console.WriteLine("Opzione non valida!");
                break;
        }
    }
}

void CustomerActions()
{
    List<Order> orders = db.Orders.ToList();

    if(orders.Count() > 0)
    {
        customerRequest = true;
        while (customerRequest)
        {
            Console.WriteLine("Di quale pacchetto vuoi vedere il dettaglio?");

            for (int i = 0; i < orders.Count(); i++)
            {
                Order order = orders[i];
                Console.WriteLine(order.ToString());
            }

            int orderId;
            try
            {
                Console.Write("Inserisci numero pacchetto: ");
                orderId = Convert.ToInt16(Console.ReadLine());

                Order order = db.Orders.Find(orderId);

                order.PrintProductsList();

                bool customerOrderConfirmRequest = true;
                while (customerOrderConfirmRequest)
                {
                    Console.WriteLine("Vuoi acquistare questo pacchetto?");
                    Console.WriteLine("[1]: sì.");
                    Console.WriteLine("[2]: no.");

                    string customerOrderConfirmResponse = Console.ReadLine();
                    switch (customerOrderConfirmResponse)
                    {
                        case "1":
                            int randomPaymentResult = new Random().Next(0, 1 + 1);

                            if(randomPaymentResult == 1)
                            {
                                Order orderToUpdate = db.Orders.Find(orderId);
                                orderToUpdate.CustomerId = 1;
                                orderToUpdate.Status = false;

                                Payment payment = new Payment();
                                payment.OrderId = orderToUpdate.Id;
                                payment.Date = DateTime.Now;
                                payment.Amount = orderToUpdate.Amount;
                                payment.Status = true;

                                db.Add(payment);
                                db.SaveChanges();

                                Console.WriteLine("Ordine effettuato con successo!");
                            }
                            else
                            {
                                Order orderToUpdate = db.Orders.Find(orderId);

                                Payment payment = new Payment();
                                payment.OrderId = orderToUpdate.Id;
                                payment.Date = DateTime.Now;
                                payment.Amount = 0;
                                payment.Status = false;

                                db.Add(payment);
                                db.SaveChanges();

                                Console.WriteLine("Si è verificato un errore durante la transazione.");
                            }
                            customerOrderConfirmRequest = false;
                            CustomerContinue();
                            break;

                        case "2":
                            customerOrderConfirmRequest = false;
                            break;

                        default:
                            Console.WriteLine("Opzione non valida!");
                            break;
                    }
                }
                CustomerContinue();
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Non puoi inserire un numero in lettere!");
            }
        }
    }
    else
    {
        Console.WriteLine("Ci dispiace ma ancora nessun pacchetto è stato caricato...");
    }
}

void EmployeeActions()
{
    emplyeeRequest = true;
    while (emplyeeRequest)
    {
        Console.WriteLine("Quale operazione vuoi effettuare?");
        Console.WriteLine("[1]: aggiungi pacchetto.");
        Console.WriteLine("[2]: cerca pacchetto.");
        Console.WriteLine("[3]: modifica pacchetto.");
        Console.WriteLine("[4]: rimuovi pacchetto.");

        string emplyeeResponse = Console.ReadLine();
        switch (emplyeeResponse)
        {
            case "1":
                CreateOrder();
                StopProgram();
                EmployeeContinue();
                break;

            case "2":
                ReadOrder();
                StopProgram();
                EmployeeContinue();
                break;

            case "3":
                UpdateOrder();
                StopProgram();
                EmployeeContinue();
                break;

            case "4":
                DeleteOrder();
                StopProgram();
                EmployeeContinue();
                break;

            default:
                Console.WriteLine("Opzione non valida!");
                break;
        }
    }
}

void CreateOrder()
{
    Console.WriteLine("Quanti prodotti vuoi inserire nel pacchetto che vuoi aggiungere?");

    int numberOfProducts = 0;
    try
    {
        Console.Write("Numero Prodotti: ");
        numberOfProducts = Convert.ToInt16(Console.ReadLine());
    }
    catch(FormatException ex)
    {
        Console.WriteLine("Non puoi inserire un numero in lettere!");
    }

    List<Product> products = db.Products.ToList();

    if (products.Count() > 0)
    {
        Console.WriteLine("---------------------------------------------------");
        for (int i = 0; i < products.Count(); i++)
        {
            Product product = products[i];

            Console.WriteLine($"[{i + 1}]: {product.ToString()}");
            Console.WriteLine("---------------------------------------------------");
        }
    }

    List<int> productsIdToAdd = new List<int>();

    for(int i = 0; i < numberOfProducts; i++)
    {
        Console.Write($"inserisci id prodotto: ");
        int productId;

        try
        {
            productId = Convert.ToInt16(Console.ReadLine());
            productsIdToAdd.Add(productId);
        }
        catch (FormatException ex)
        {
            Console.WriteLine("Non puoi inserire un numero in lettere!");
        }
    }

    Order newOrder = new Order();
    newOrder.EmployeeId = 1;
    newOrder.Date = DateTime.Now;

    newOrder.Products = new List<Product>();

    foreach (int id in productsIdToAdd)
    {
        Product product = db.Products.Find(id);
        newOrder.Products.Add(product);
    }

    newOrder.Amount = newOrder.CalcTotalAmount();
    newOrder.Status = true;

    db.Orders.Add(newOrder);
    db.SaveChanges();

    Console.WriteLine("Pacchetto aggiunto correttamente.");
}

void ReadOrder()
{
    Console.WriteLine("Inserisci l'id del pacchetto che vuoi cercare");

    int orderId;
    try
    {
        Console.Write("Id pacchetto: ");
        orderId = Convert.ToInt16(Console.ReadLine());

        //List<Product> order = db.Orders.Include(p => p.Products).ToList();

        //Console.WriteLine(order.ToString());

        //Console.WriteLine("Lista prodotti presenti nel pacchetto: ");

        //List<Product> students = 
    }
    catch (FormatException ex)
    {
        Console.WriteLine("Non puoi inserire un numero in lettere!");
    }
}

void UpdateOrder()
{
    Console.WriteLine("Inserisci l'id del pacchetto che vuoi modificare.");

    int orderId;
    try
    {
        Console.Write("Id pacchetto: ");
        orderId = Convert.ToInt16(Console.ReadLine());

        Order order = db.Orders.Find(orderId);

        Console.WriteLine(order.ToString());

        Console.WriteLine("Lista prodotti presenti nel pacchetto: ");

        order.PrintProductsList();

        // da continuare

        Console.WriteLine("Pacchetto modificato correttamente.");
    }
    catch (FormatException ex)
    {
        Console.WriteLine("Non puoi inserire un numero in lettere!");
    }
}

void DeleteOrder()
{
    Console.WriteLine("Inserisci l'id del pacchetto che vuoi eliminare.");

    int orderId;
    try
    {
        Console.Write("Id pacchetto: ");
        orderId = Convert.ToInt16(Console.ReadLine());

        Order order = db.Orders.Find(orderId);

        db.Orders.Remove(order);
        db.SaveChanges();

        Console.WriteLine("Pacchetto eliminato correttamente.");
    }
    catch (FormatException ex)
    {
        Console.WriteLine("Non puoi inserire un numero in lettere!");
    }
}

void EmployeeContinue()
{
    bool employeeContinueRequest = true;
    while (employeeContinueRequest)
    {
        Console.WriteLine("Vuoi fare altro?");
        Console.WriteLine("[1]: sì.");
        Console.WriteLine("[2]: no.");

        string employeeContinueResponse = Console.ReadLine();
        switch (employeeContinueResponse)
        {
            case "1":
                employeeContinueRequest = false;
                break;

            case "2":
                employeeContinueRequest = false;
                emplyeeRequest = false;
                break;

            default:
                Console.WriteLine("Opzione non valida!");
                break;
        }
    }
}

void CustomerContinue()
{
    bool customerContinueRequest = true;
    while (customerContinueRequest)
    {
        Console.WriteLine("Vuoi fare un altro ordine?");
        Console.WriteLine("[1]: sì.");
        Console.WriteLine("[2]: no.");

        string customerContinueResponse = Console.ReadLine();
        switch (customerContinueResponse)
        {
            case "1":
                customerContinueRequest = false;
                break;

            case "2":
                customerContinueRequest = false;
                customerRequest = false;
                break;

            default:
                Console.WriteLine("Opzione non valida!");
                break;
        }
    }
}

void StopProgram()
{
    bool stopProgramRequest = true;
    while (stopProgramRequest)
    {
        Console.WriteLine("Vuoi terminare il programma?");
        Console.WriteLine("[1]: sì.");
        Console.WriteLine("[2]: no.");

        string stopProgramResponse = Console.ReadLine();
        switch (stopProgramResponse)
        {
            case "1":
                Console.WriteLine("Grazie e arrivederci!");
                stopProgramRequest = false;
                runProgram = false;
                break;

            case "2":
                stopProgramRequest = false;
                break;

            default:
                Console.WriteLine("Opzione non valida!");
                break;
        }
    }
}