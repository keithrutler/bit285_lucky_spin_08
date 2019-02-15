﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore; //add the Microsoft.EntityFrameworkCore
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LuckySpin.Models;
using LuckySpin.ViewModels;

namespace LuckySpin.Controllers
{
    public class SpinnerController : Controller
    {
        private LuckySpinDataContext _dbc;
        Random random;

        /***
         * Controller Constructor
         *   Inject the LuckySpinDataContext        
         */
        public SpinnerController(LuckySpinDataContext dbc)
        {
            random = new Random();
            _dbc = dbc;
        }

        /***
         * Entry Page Action
         **/

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Player player)
        {
            if (!ModelState.IsValid) { return View(); }

            //Add the Player to the DB and save the changes
            _dbc.Players.Add(player);
            _dbc.SaveChanges();

            //pass the player.Id to the SpinIt action
            return RedirectToAction("SpinIt", new { id = player.Id });
        }

        /***
         * SpinIt Action
         **/

        public IActionResult SpinIt(int id) //change parameter to receive players Id
        {
            //Get the player with the given Id using the Players DbSet Find(Id) method
            var currentPlayer = _dbc.Players.Include(p => p.Spins).Single(p => p.Id == id);
            if (currentPlayer == null) { return View("Index"); }

            //Build a new SpinItViewModel object with data from the Player and spin
            SpinViewModel spinVM = new SpinViewModel()
            {
                FirstName = currentPlayer.FirstName,
                Balance = currentPlayer.Balance,
                Luck = currentPlayer.Luck,
                A = random.Next(1, 10),
                B = random.Next(1, 10),
                C = random.Next(1, 10)
            };

            spinVM.IsWinning = (spinVM.A == spinVM.Luck || spinVM.B == spinVM.Luck || spinVM.C == spinVM.Luck);

            //TODO : Add LuckySpin Game Logic (review flow chart for details)
            if(spinVM.Balance < 0.50m)
            {
                return RedirectToAction("LuckList", id);
            }
            else
            {
                spinVM.Balance = spinVM.Balance - 0.50m;
                if (spinVM.IsWinning)
                    spinVM.Balance = spinVM.Balance + 1.00m;
            }
            //Prepare the ViewBag
            if (spinVM.IsWinning)
            {
                ViewBag.Display = "block";
            }
                ViewBag.Display = "none";
            //Use current player id for the link to LinkList 
            //   (see the <a href> for "Current Balance" in the SpinIt.cshtml file)
            ViewBag.PlayerId = id;

            //Add the new Spin to the __current player's__ Spins collection
            var spin = new Spin { IsWinning = spinVM.IsWinning };
            currentPlayer.Spins.Add(spin);

            //Update the database - only done once after all changes stored in Entities
            _dbc.SaveChanges();

            return View("SpinIt", spinVM);
        }

        /***
         * LuckList Action
         **/

        public IActionResult LuckList(int id)
        {
            //Get the current player including their Spins list
            var currentPlayer = _dbc.Players.Include(p => p.Spins).Single(p => p.Id == id);

            //Send the player's Spins to the View
            return View(currentPlayer.Spins);
        }

        /***
         *  Start the Game Again
         * */
         public IActionResult StartOver(int id)
        {
            //TODO: Remove Player and Update the Datbase
            //_dbc.Players.Remove(id);
            _dbc.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}

