﻿using BlackjackStrategies.Application;
using BlackjackStrategies.Application.BetService;
using BlackjackStrategies.Application.Strategies;
using BlackjackStrategies.Infrastructure;
using BlackjackStrategies.UI;

var startingAmount = 200M;
var bettingSize = 15M;
var numberOfGames = 1000;
var numberOfDecks = 6;
var strategyType = StrategyType.Martingale;

var gameAnalyser = new GameAnalyser();
var gamePrinter = new GamePrinter(gameAnalyser);
var betServiceFactory = new BetServiceFactory();
var gameSimulator = new GameSimulator(new BasicStrategyPlayerService(), betServiceFactory);
var csvWriter = new CsvWriter();

var gameOutcomes = gameSimulator.Simulate(numberOfDecks, numberOfGames, startingAmount, bettingSize, strategyType);
gamePrinter.Print(gameOutcomes.ToArray(), numberOfDecks, startingAmount);

csvWriter.WriteToCsv(gameOutcomes, "../../../gameOutcomes.csv");
