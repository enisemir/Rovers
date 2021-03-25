using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MarsRover.Models;
using MarsRover.Enum;

namespace MarsRover.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Explore(RoverInputModel roverInputModel)
        {
            var resultModel = new ResultModel();
            try
            {
                var coordinates = SplitCoordinate(roverInputModel.Plateau);


                var firstRoverPos = SplitCoordinate(roverInputModel.FirstRoverPos);
                //Yönü ayırdık
                var firtRoverNavigate = firstRoverPos.LastOrDefault();
                var firstIndex = firstRoverPos.Count() - 1;
                //x ve y aldık kaldı
                firstRoverPos.RemoveAt(firstIndex);
                var firstRoverMove = SplitCoordinate(roverInputModel.FirstRoverMove);


                var secondRoverPos = SplitCoordinate(roverInputModel.SecondRoverPos);
                //Yönü ayırdık
                var secondRoverNavigate = secondRoverPos.LastOrDefault();
                var secondIndex = secondRoverPos.Count() - 1;
                //x ve y aldık kaldı
                secondRoverPos.RemoveAt(secondIndex);


                var secondRoverMove = SplitCoordinate(roverInputModel.SecondRoverMove);

               
                resultModel.FirstRoverResult = FindDestination(coordinates, firstRoverPos, firtRoverNavigate, firstRoverMove);
                resultModel.SecondRoverResult = FindDestination(coordinates, secondRoverPos, secondRoverNavigate, secondRoverMove);
            }
            catch (Exception ex)
            {

                throw new Exception("hata" + ex);
            }           
            return View(resultModel);
        }



        //Boşluk olmadan girilenleri tek karakter olarak listeye ekleyip döner.
        public static List<string> SplitCoordinate(string coordinate)
        {
            try
            {
                List<string> datalist = new List<string>();
                datalist.AddRange(coordinate.Select(c => c.ToString()));
                datalist = datalist.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                return datalist;
            }
            catch (Exception ex)
            {

                throw new Exception("split hata" +ex);
            }            
        }
        //Yeni Yönü bulalım.
        public static string FindNavigate(string firtRoverNavigate, string item)
        {
            Direction.Directions myEnum = (Direction.Directions)Enum.Direction.Directions.Parse(typeof(Direction.Directions), firtRoverNavigate);
            Direction.Directions myEnum2 = Direction.Directions.N;
            if (item == "R")
            {
                myEnum2 = (Direction.Directions)(((int)myEnum + 3) % 4);
            }
            else
            {
                myEnum2 = (Direction.Directions)(((int)myEnum +1) % 4);
            }
             
            return myEnum2.ToString();
        }
        //Yeni X pozisyonunu bulalım.
        public static int FindXPosition(string firtRoverNavigate, int maxXPlateau, int RoverPosX)
        {
            var newPos = 0;
            if (firtRoverNavigate == "E")
            {
                newPos = RoverPosX + 1;
            }
            else
            {
                newPos = RoverPosX - 1;
            }
            if (RoverPosX < 0 || RoverPosX > maxXPlateau)
            {
                throw new Exception("Alanın dışına çıkıldı!!!");
            }
            return newPos;
        }
        //Yeni Y pozisyonunu bulalım.
        public static int FindYPosition(string firtRoverNavigate, int maxYPlateau, int RoverPosY)
        {
            var newPos = 0;
            if (firtRoverNavigate == "S")
            {
                newPos = RoverPosY - 1;
            }
            else
            {
                newPos = RoverPosY + 1;
            }
            if (RoverPosY < 0 || RoverPosY > maxYPlateau)
            {
                throw new Exception("Alanın dışına çıkıldı!!!");
            }
            return newPos;
        }
        //Son pozisyonunu bulalım.
        string firstRoverDestination = "";
        public string FindDestination(List<string> coordinates, List<string> roverPos, string roverNavigate, List<string> firstRoverMove)
        {
            var maxXPlateau = Convert.ToInt32(coordinates.FirstOrDefault());
            var maxYPlateau = Convert.ToInt32(coordinates.LastOrDefault());

            var RoverPosX = Convert.ToInt32(roverPos.FirstOrDefault());
            var RoverPosY = Convert.ToInt32(roverPos.LastOrDefault());
            try
            {
                
                foreach (var item in firstRoverMove)
                {
                    if (item == "R" || item == "L")
                    {
                        roverNavigate = FindNavigate(roverNavigate, item);
                    }
                    else
                    {
                        if (roverNavigate == "E" || roverNavigate == "W")
                        {
                            RoverPosX = FindXPosition(roverNavigate, maxXPlateau, RoverPosX);
                        }
                        else
                        {
                            RoverPosY = FindYPosition(roverNavigate, maxYPlateau, RoverPosY);
                        }
                        if (!string.IsNullOrEmpty(firstRoverDestination))
                        {
                            ControlRovers(firstRoverDestination, RoverPosX, RoverPosY);

                        }
                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception("FindDestination hata!!"+ex);
            }
            if (string.IsNullOrEmpty(firstRoverDestination))
            {
                firstRoverDestination = RoverPosX.ToString() + RoverPosY.ToString();
            }            
            return RoverPosX.ToString() + " " + RoverPosY.ToString() + " " + roverNavigate; 
        }
        public bool ControlRovers(string firstRoverDestination,int RoverPosX,int RoverPosY)
        {
            var secondRoverDestination = RoverPosX.ToString() + RoverPosY.ToString();
            if (firstRoverDestination == secondRoverDestination)
            {
                throw new Exception("Roverlar aynı pozisyonda olamaz!!");
            }
            return true;
        }
    }
}
