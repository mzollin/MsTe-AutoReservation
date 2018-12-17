using System;
using System.Collections.Generic;
using System.Linq;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Dal.Entities;

namespace AutoReservation.Service.Wcf
{
    public static class DtoConverter
    {
        #region Auto
        private static Auto GetAutoInstance(AutoDto dto)
        {
            if (dto.AutoClass == AutoKlasse.StandardAuto) { return new StandardAuto(); }
            if (dto.AutoClass == AutoKlasse.MittelklasseAuto) { return new MittelklasseAuto(); }
            if (dto.AutoClass == AutoKlasse.LuxusklasseAuto) { return new LuxusklasseAuto(); }
            throw new ArgumentException("Unknown AutoDto implementation.", nameof(dto));
        }
        public static Auto ConvertToEntity(this AutoDto dto)
        {
            if (dto == null) { return null; }

            Auto auto = GetAutoInstance(dto);
            auto.Id = dto.Id;
            auto.Brand = dto.Brand;
            auto.DailyRate = dto.DailyRate;
            auto.RowVersion = dto.RowVersion;

            if (auto is LuxusklasseAuto)
            {
                ((LuxusklasseAuto)auto).BaseRate = dto.BaseRate;
            }
            return auto;
        }
        public static AutoDto ConvertToDto(this Auto entity)
        {
            if (entity == null) { return null; }

            AutoDto dto = new AutoDto
            {
                Id = entity.Id,
                Brand = entity.Brand,
                DailyRate = entity.DailyRate,
                RowVersion = entity.RowVersion
            };

            if (entity is StandardAuto) { dto.AutoClass = AutoKlasse.StandardAuto; }
            if (entity is MittelklasseAuto) { dto.AutoClass = AutoKlasse.MittelklasseAuto; }
            if (entity is LuxusklasseAuto)
            {
                dto.AutoClass = AutoKlasse.LuxusklasseAuto;
                dto.BaseRate = ((LuxusklasseAuto)entity).BaseRate;
            }


            return dto;
        }
        public static List<Auto> ConvertToEntities(this IEnumerable<AutoDto> dtos)
        {
            return ConvertGenericList(dtos, ConvertToEntity);
        }
        public static List<AutoDto> ConvertToDtos(this IEnumerable<Auto> entities)
        {
            return ConvertGenericList(entities, ConvertToDto);
        }
        #endregion

        #region Kunde
        public static Kunde ConvertToEntity(this KundeDto dto)
        {
            if (dto == null) { return null; }

            return new Kunde
            {
                Id = dto.Id,
                Surname = dto.Surname,
                FirstName = dto.FirstName,
                Birthday = dto.Birthday,
                RowVersion = dto.RowVersion
            };
        }
        public static KundeDto ConvertToDto(this Kunde entity)
        {
            if (entity == null) { return null; }

            return new KundeDto
            {
                Id = entity.Id,
                Surname = entity.Surname,
                FirstName = entity.FirstName,
                Birthday = entity.Birthday,
                RowVersion = entity.RowVersion
            };
        }
        public static List<Kunde> ConvertToEntities(this IEnumerable<KundeDto> dtos)
        {
            return ConvertGenericList(dtos, ConvertToEntity);
        }
        public static List<KundeDto> ConvertToDtos(this IEnumerable<Kunde> entities)
        {
            return ConvertGenericList(entities, ConvertToDto);
        }
        #endregion
        #region Reservation
        public static Reservation ConvertToEntity(this ReservationDto dto)
        {
            if (dto == null) { return null; }

            Reservation reservation = new Reservation
            {
                ReservationsNr = dto.ReservationsNr,
                From = dto.From,
                To = dto.To,
                AutoId = dto.Car.Id,
                KundeId = dto.Customer.Id,
                RowVersion = dto.RowVersion
            };

            return reservation;
        }
        public static ReservationDto ConvertToDto(this Reservation entity)
        {
            if (entity == null) { return null; }

            return new ReservationDto
            {
                ReservationsNr = entity.ReservationsNr,
                From = entity.From,
                To = entity.To,
                RowVersion = entity.RowVersion,
                Car = ConvertToDto(entity.Auto),
                Customer = ConvertToDto(entity.Kunde)
            };
        }
        public static List<Reservation> ConvertToEntities(this IEnumerable<ReservationDto> dtos)
        {
            return ConvertGenericList(dtos, ConvertToEntity);
        }
        public static List<ReservationDto> ConvertToDtos(this IEnumerable<Reservation> entities)
        {
            return ConvertGenericList(entities, ConvertToDto);
        }
        #endregion

        private static List<TTarget> ConvertGenericList<TSource, TTarget>(this IEnumerable<TSource> source, Func<TSource, TTarget> converter)
        {
            if (source == null) { return null; }
            if (converter == null) { return null; }

            return source.Select(converter).ToList();
        }
    }

}
