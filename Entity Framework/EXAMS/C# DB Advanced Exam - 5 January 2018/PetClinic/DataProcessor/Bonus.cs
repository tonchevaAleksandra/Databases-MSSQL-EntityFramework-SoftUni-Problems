using System.ComponentModel.DataAnnotations;
using System.Linq;
using PetClinic.Data;
using PetClinic.Models;

namespace PetClinic.DataProcessor
{
    public class Bonus
    {
        public static string UpdateVetProfession(PetClinicContext context, string phoneNumber, string newProfession)
        {
            Vet vet = context.Vets.FirstOrDefault(x => x.PhoneNumber.Equals(phoneNumber));

            if (vet==null)
            {
                return $"Vet with phone number {phoneNumber} not found!";
            }

            string oldProfession = vet.Profession;
            vet.Profession = newProfession;
            context.SaveChanges();

            return $"“{vet.Name}'s profession updated from {oldProfession} to {vet.Profession}.";
        }
    }
}