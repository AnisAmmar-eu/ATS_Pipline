using Core.Entities.Alarms.AlarmsC.Services;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.Packets.Services;
using Core.Entities.User.Services.Roles;
using Core.Shared.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Services.Background
{
    public class JokeService : BackgroundService
    {
        private readonly IServiceScopeFactory _factory;
        private readonly ILogger<PurgeService> _logger;
        private readonly IConfiguration _configuration;

        public JokeService(
            ILogger<PurgeService> logger,
            IServiceScopeFactory factory,
            IConfiguration configuration)
        {
            _logger = logger;
            _factory = factory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();

            int jokeTimer = _configuration.GetValueWithThrow<int>("JokeTimer");
            using PeriodicTimer timer = new(TimeSpan.FromSeconds(jokeTimer));

            while (await timer.WaitForNextTickAsync(stoppingToken)
                && !stoppingToken.IsCancellationRequested)
            {
                try
                {
                    string joke = GetJoke();
                    _logger.LogInformation("{Joke}", joke);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        "Failed to execute PurgeService with exception message {message}.",
                        ex.Message);
                }
            }
        }

        public string GetJoke()
        {
            Joke joke = _jokes.ElementAt(
                Random.Shared.Next(_jokes.Count));

            return $"{joke.Setup}{Environment.NewLine}{joke.Punchline}";
        }

        // Programming jokes borrowed from:
        // https://github.com/eklavyadev/karljoke/blob/main/source/jokes.json
        private readonly HashSet<Joke> _jokes = new()
        {
            new("What's the best thing about a Boolean?", "Even if you're wrong, you're only off by a bit."),
            new ("What's the object-oriented way to become wealthy?", "Inheritance"),
            new ("Why did the programmer quit their job?", "Because they didn't get arrays."),
            new ("Why do programmers always mix up Halloween and Christmas?", "Because Oct 31 == Dec 25"),
            new ("How many programmers does it take to change a lightbulb?", "None that's a hardware problem"),
            new (
                "If you put a million monkeys at a million keyboards, one of them will eventually write a Java program",
                "the rest of them will write Perl"),
            new ("['hip', 'hip']", "(hip hip array)"),
            new ("To understand what recursion is...", "You must first understand what recursion is"),
            new ("There are 10 types of people in this world...", "Those who understand binary and those who don't"),
            new ("Which song would an exception sing?", "Can't catch me - Avicii"),
            new ("Why do Java programmers wear glasses?", "Because they don't C#"),
            new ("How do you check if a webpage is HTML5?", "Try it out on Internet Explorer"),
            new ("A user interface is like a joke.", "If you have to explain it then it is not that good."),
            new ("I was gonna tell you a joke about UDP...", "...but you might not get it."),
            new ("The punchline often arrives before the set-up.", "Do you know the problem with UDP jokes?"),
            new (
                "Why do C# and Java developers keep breaking their keyboards?",
                "Because they use a strongly typed language."),
            new ("Knock-knock.", "A race condition. Who is there?"),
            new ("What's the best part about TCP jokes?", "I get to keep telling them until you get them."),
            new (
                "A programmer puts two glasses on their bedside table before going to sleep.",
                "A full one, in case they gets thirsty, and an empty one, in case they don’t."),
            new (
                "There are 10 kinds of people in this world.",
                "Those who understand binary, those who don't, and those who weren't expecting a base 3 joke."),
            new ("What did the router say to the doctor?", "It hurts when IP."),
            new ("An IPv6 packet is walking out of the house.", "He goes nowhere."),
            new("3 SQL statements walk into a NoSQL bar. Soon, they walk out", "They couldn't find a table."),
        };

        readonly record struct Joke(string Setup, string Punchline);
    }
}