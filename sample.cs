
using Microsoft.AspNetCore.Builder;

namespace Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*
            based on args or configmap etc. choose adequate startup(), or do it by hand by calling the right function as follows
            */
            DefaultRestApiStartup();
        }

        private static void DefaultRestApiStartup()
        {
            var builder = WebApplication.CreateBuilder(args);
            addServices(builder);
            var app = builder.Build();
            useServices(app);
            app.Run();

            void addServices(builder)
            {
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
            }

            void useServices(app)
            {
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
                app.UseHttpsRedirection();
                app.UseAuthorization();
                app.MapControllers();
                // DI
            }
        }

        private static void DefaultGraphQLStartup()
        {

        }
    }
}







//Core.Domain
//domain contains business specific rules
//=====================================================================================================================================================================
class Pizza
{
    double Price;
}


//Core.Interactor #alias Usecase
//=====================================================================================================================================================================
class OrderPizzaInteractor
{
    OrderPizzaController _orderPizzaController; //desired impl injected
    ICalcDiscountPort _calcDiscountPort; //desired impl injected

    Response Act(Request request)
    {
        var price = _calcDiscountPort.Calc(Pizza pizza)
        var res = 
        return res
    }
}

//interactors(usecases) have their own Request Response model to fully decouple it from controller 
//(because controller's request respone is more like a contract to the outer world and is more likely to change)
class Request 
{
    Pizza pizza
}

class Response 
{
    //best would be to use errorOr or Result<error,value> so some kind of discunion, lets stay with isSuccess for now
    bool isSuccess;
}


//Core.Ports
//=====================================================================================================================================================================
//ports are abstractions which have their corresponding implementations as adapters
//ports-adapters approach ensures swappable modular design (for example StorePizzaPort could be implemented
//as StorePizzaMongoAdapter or StorePizzaLocalDriveAdapter or StorePizzaMySqlAdapter)
interface CalcDiscountPort //or ICalcDiscountPort (here in c# abstract classes and interfaces can be ports)
{
    public double Calc(Pizza pizza); //could be generic using TPizza
}


//Infra.Adapters
//=====================================================================================================================================================================
//adapters are concrete implementations of ports
//adapters contain business specific rules
class CalcDiscountBlackNovemberAdapter : CalcDiscountPort
{
    public double Calc(Pizza pizza)
    {
        //does some stuff
    }
}


//Infra.Controller
//=====================================================================================================================================================================
//controllers in a classical sense, we're not talking about MVC pattern controllers
//controllers decide WHEN and WHAT should be executed and do mappings, 
//controllers are using DTO-s which are different from domain objects (usecases use domain objects controllers use DTO-s)
//controllers are sometimes also presenters 
//controller could be a Console input or a GPIO but this time its a webserver listening for http requests
class OrderPizzaController : WebController
{
    [method=GET, route=/orderpizza?name=songoku]    //nemtom fejből xd
    public double Control(Pizza pizza)
    {
        //calls desired interactor(alias usecase)
        //optionally object mappoing and works as presenter too
    }
}


//Infra.Setup
//=====================================================================================================================================================================
//does configuration, (DI,middlewares,handles external configs)
//main chooses the right one and calls it
public static void RunDefaultSetup()    //could be a class with Run() method and use polimorphism
{
}
//or maybe
public static void RunSpecialSetup()
{
}
//or maybe
public static void RunTestingSetup()
{
}


//so Program.cs would be this simple
public static void main(string[] args)
{
    RunDefaultSetup();
}



//summary
//setup calls controller which calls interactor(usecase) which does the job (using ports if any, and the domain objects)
