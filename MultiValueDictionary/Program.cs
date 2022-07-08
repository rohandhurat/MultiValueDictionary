using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using MultiValueDictionary;

namespace Application
{
    class Program
    {
        static void Main(string[] args)
        {
            //this function will inject the dependency,for concrete
            //multifalurdictionary and the for base dictionary used underneath,
            ConfigureServices();

            //resolve and instantiate dictionary
            var _dictionary = IoC.Get<IMultiValueDictionary<string, string>>();

            string input;
            do
            {
                Console.Write("> ");
                input = Console.ReadLine();
                //input in whitespace seperated e.g. Command param1 param2
                var splitInput = input.Split(' ');

                switch (splitInput[0])
                {
                    //input KEYS
                    case "KEYS":
                        if (splitInput.Length != 1)
                            Console.WriteLine(") ERROR, Incorrect number of arguments");
                        else
                        {
                            var keys = _dictionary.Keys();
                            if (keys == null)
                                Console.WriteLine("(empty set)");
                            else
                            {
                                int keyNum = 1;
                                foreach (var key in keys)
                                {
                                    Console.WriteLine("{0}) {1}", keyNum++, key);
                                }
                            }
                        }
                        break;
                    //input MEMBERS param1
                    case "MEMBERS":
                        if (splitInput.Length != 2)
                            Console.WriteLine(") ERROR, Incorrect number of arguments");
                        else
                        {
                            try
                            {
                                var members = _dictionary.Members(splitInput[1]);
                                int memNum = 1;
                                foreach (var member in members)
                                {
                                    Console.WriteLine("{0}) {1}", memNum++, member);
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(") ERROR, " + ex.Message);
                            }
                        }
                        break;
                    //input: ADD param1 param2
                    case "ADD":
                        if (splitInput.Length != 3)
                            Console.WriteLine(") ERROR, Incorrect number of arguments");
                        else
                        {
                            try
                            {
                                _dictionary.Add(splitInput[1], splitInput[2]);
                                Console.WriteLine(") Added");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(") ERROR, " + ex.Message);
                            }
                        }
                        break;
                    //input REMOVE param1 param2
                    case "REMOVE":
                        if (splitInput.Length != 3)
                            Console.WriteLine(") ERROR, Incorrect number of arguments");
                        else
                        {
                            try
                            {
                                _dictionary.Remove(splitInput[1], splitInput[2]);
                                Console.WriteLine(") Removed");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(") ERROR, " + ex.Message);
                            }
                        }
                        break;
                    //input REMOVEALL param1
                    case "REMOVEALL":
                        if (splitInput.Length != 2)
                            Console.WriteLine(") ERROR, Incorrect number of arguments");
                        else
                        {
                            try
                            {
                                _dictionary.RemoveAll(splitInput[1]);
                                Console.WriteLine(") Removed");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(") ERROR, " + ex.Message);
                            }
                        }
                        break;
                    //input: CLEAR
                    case "CLEAR":
                        if (splitInput.Length != 1)
                            Console.WriteLine(") ERROR, Incorrect number of arguments");
                        else
                        {
                            _dictionary.Clear();
                            Console.WriteLine(") Cleared");
                        }
                        break;
                    //input: KEYEXISTS param1
                    case "KEYEXISTS":
                        if (splitInput.Length != 2)
                            Console.WriteLine(") ERROR, Incorrect number of arguments");
                        else
                        {
                            Console.WriteLine(") " + _dictionary.KeyExistes(splitInput[1]));
                        }
                        break;
                    //input:MEMBEREXISTS param1 param2
                    case "MEMBEREXISTS":
                        if (splitInput.Length != 3)
                            Console.WriteLine(") ERROR, Incorrect number of arguments");
                        else
                        {
                            Console.WriteLine(") " + _dictionary.MemberExistes(splitInput[1], splitInput[2]));
                        }
                        break;
                    //input: ALLMEMBERS
                    case "ALLMEMBERS":
                        if (splitInput.Length != 1)
                            Console.WriteLine(") ERROR, Incorrect number of arguments");
                        else
                        {
                            var members = _dictionary.AllMembers();
                            if (members == null)
                                Console.WriteLine("(empty set)");
                            else
                            {
                                int memberNum = 1;
                                foreach (var member in members)
                                {
                                    Console.WriteLine("{0}) {1}", memberNum++, member);
                                }
                            }
                        }
                        break;
                    //input: ITEMS
                    case "ITEMS":
                        if (splitInput.Length != 1)
                            Console.WriteLine(") ERROR, Incorrect number of arguments");
                        else
                        {
                            var items = _dictionary.Items();
                            if (items == null)
                                Console.WriteLine("(empty set)");
                            else
                            {
                                int itemNum = 1;
                                foreach (var key in items.Keys)
                                {
                                    foreach (var member in items[key])
                                    {
                                        Console.WriteLine("{0}) {1}: {2}", itemNum++, key, member);

                                    }

                                }
                            }
                        }
                        break;
                    case "EXIT":
                        break;
                    default:
                        Console.WriteLine(") ERROR, Unsupported operation; please try again");
                        break;
                }
            } while (!input.Equals("EXIT"));
        }

        private static void ConfigureServices()
        {
            //register IMultiValueDictionary with concrete MultiValueDictionary
            IoC.Register<IMultiValueDictionary<string, string>, MultiValueDictionary<string, string>>();

            //register IDictionary with concrete BaseDictionary
            IoC.Register<IDictionary<string, ICollection<string>>, BaseDictionary<string, ICollection<string>>>();
        }
    }
}
