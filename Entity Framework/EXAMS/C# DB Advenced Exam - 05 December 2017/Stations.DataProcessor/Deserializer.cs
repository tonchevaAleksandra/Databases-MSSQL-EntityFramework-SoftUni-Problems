using System;
using Stations.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using Stations.DataProcessor.Dto.Import;
using Stations.Models;
using Stations.Models.Enums;

namespace Stations.DataProcessor
{
    public static class Deserializer
    {
        private const string FailureMessage = "Invalid data format.";

        private const string SuccessMessage = "Record {0} successfully imported.";

        public static string ImportStations(StationsDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportStationDto[] stationDtos = JsonConvert.DeserializeObject<ImportStationDto[]>(jsonString);

            List<Station> stationsToAdd = new List<Station>();

            foreach (var stationDto in stationDtos)
            {
                if (!IsValid(stationDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (stationsToAdd.Any(x => x.Name == stationDto.Name))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }
                Station station = new Station()
                {
                    Name = stationDto.Name,
                    Town = stationDto.Town ?? stationDto.Name
                };

                stationsToAdd.Add(station);

                sb.AppendLine(string.Format(SuccessMessage, station.Name));
            }

            context.Stations.AddRange(stationsToAdd);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportClasses(StationsDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportSeatingClassDto[] classDtos = JsonConvert.DeserializeObject<ImportSeatingClassDto[]>(jsonString);

            List<SeatingClass> seatingClassesToAdd = new List<SeatingClass>();

            foreach (var classDto in classDtos)
            {
                if (!IsValid(classDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (seatingClassesToAdd.Any(x => x.Abbreviation.Equals(classDto.Abbreviation)) || seatingClassesToAdd.Any(x => x.Name == classDto.Name))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                SeatingClass seatingClass = new SeatingClass()
                {
                    Name = classDto.Name,
                    Abbreviation = classDto.Abbreviation
                };

                seatingClassesToAdd.Add(seatingClass);
                sb.AppendLine(string.Format(SuccessMessage, seatingClass.Name));
            }

            context.SeatingClasses.AddRange(seatingClassesToAdd);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportTrains(StationsDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportTrainDto[] trainDtos = JsonConvert.DeserializeObject<ImportTrainDto[]>(jsonString);

            List<Train> trainsToAdd = new List<Train>();

            foreach (var trainDto in trainDtos)
            {
                if (!IsValid(trainDto) || !trainDto.Seats.All(IsValid))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (trainsToAdd.Any(x => x.TrainNumber == trainDto.TrainNumber))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (!trainDto.Seats
                    .All(s => context.SeatingClasses
                        .Any(sc => sc.Name == s.Name &&
                                   sc.Abbreviation == s.Abbreviation)))
                {
                    sb.AppendLine(FailureMessage);

                    continue;
                }

                Train train = new Train()
                {
                    TrainNumber = trainDto.TrainNumber,
                    Type = trainDto.Type ?? TrainType.HighSpeed
                };

                train.TrainSeats = trainDto.Seats
                    .Select(x => new TrainSeat()
                    {
                        SeatingClass =
                            context.SeatingClasses.FirstOrDefault(s =>
                                s.Name == x.Name && s.Abbreviation == x.Abbreviation),
                        Quantity = x.Quantity.Value,
                        Train = train
                    }).ToList();

                trainsToAdd.Add(train);
                sb.AppendLine(string.Format(SuccessMessage, train.TrainNumber));
            }

            context.Trains.AddRange(trainsToAdd);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportTrips(StationsDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

        }

        public static string ImportCards(StationsDbContext context, string xmlString)
        {
            return null;
        }

        public static string ImportTickets(StationsDbContext context, string xmlString)
        {
            return null;
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}