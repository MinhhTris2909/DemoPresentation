using System.Collections;
using System.Collections.Generic;

namespace DemoPresentation.Repository.IRepository
{
    public interface IRepository
    {
        IEnumerable<Reservation> Reservations { get; }

        Reservation this[int id ] { get;}

        Reservation AddReservation (Reservation reservation);

        Reservation UpdateReservation (Reservation reservation);

        void DeleteReservation (int id);
    }
}
