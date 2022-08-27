using client;

myClient client = new myClient();



client.SendMessage("Baslayalim Wellcome, what is your name ?");
string? name = Console.ReadLine();




client.SendMessage("Can you tell me your surname " + name + "?");
string? surname = Console.ReadLine();





client.SendMessage(name + " " + surname + " what is your email address");
string? email = Console.ReadLine();


client.SendMessage("I'm registering to the " + name + " system" + Environment.NewLine + name + Environment.NewLine + surname + Environment.NewLine + email);


Console.ReadKey();