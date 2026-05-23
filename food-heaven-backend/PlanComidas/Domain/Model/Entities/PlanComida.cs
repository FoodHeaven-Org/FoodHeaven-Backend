using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using food_heaven_backend.Shared.Domain.Model.Entities;

namespace food_heaven_backend.PlanComidas.Domain.Model.Entities;

[Table("PlanComida")]
public class PlanComida : BaseEntity
{
    private const int DaysInWeek = 7;
    private const int MealTypesPerDay = 3;
    private const int WeeklyMealSlots = DaysInWeek * MealTypesPerDay;
    private const string BreakfastDefaultSchedule = "07:00-09:00";
    private const string LunchDefaultSchedule = "12:00-14:00";
    private const string DinnerDefaultSchedule = "19:00-21:00";
    private const char DeliveryScheduleSeparator = '|';

    public static string DefaultDeliverySchedulesStorageValue =>
        SerializeDeliverySchedules(CreateDefaultDeliverySchedules());

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("fecha_inicio")]
    public DateTime FechaInicio { get; set; }

    [Column("fecha_fin")]
    public DateTime FechaFin { get; set; }

    [Column("lista_comidas")]
    public int[] ListaComidas { get; set; } = Array.Empty<int>();

    [Column("horarios_entrega")]
    public string[] HorariosEntrega { get; set; } = CreateDefaultDeliverySchedules();

    public PlanComida(int idUsuario, DateTime fechaInicio, DateTime fechaFin, int[] listaComidas, string[]? horariosEntrega = null)
    {
        UpdateDetails(idUsuario, fechaInicio, fechaFin, listaComidas, horariosEntrega);
    }

    public PlanComida() { }

    public void UpdateDetails(int idUsuario, DateTime fechaInicio, DateTime fechaFin, int[] listaComidas, string[]? horariosEntrega = null)
    {
        if (idUsuario <= 0) throw new ArgumentException("IdUsuario must be greater than zero.", nameof(idUsuario));
        if (fechaInicio == default) throw new ArgumentException("FechaInicio is required.", nameof(fechaInicio));
        if (fechaFin <= fechaInicio) throw new ArgumentException("FechaFin must be after FechaInicio.", nameof(fechaFin));
        if (listaComidas is not { Length: WeeklyMealSlots }) throw new ArgumentException("ListaComidas must contain exactly 21 meal slots.", nameof(listaComidas));
        if (listaComidas.Any(id => id < 0)) throw new ArgumentException("ListaComidas cannot contain negative meal ids.", nameof(listaComidas));

        IdUsuario = idUsuario;
        FechaInicio = fechaInicio;
        FechaFin = fechaFin;
        ListaComidas = listaComidas;
        HorariosEntrega = NormalizeDeliverySchedules(horariosEntrega);
    }

    public static string[] CreateDefaultDeliverySchedules()
    {
        var schedules = new string[WeeklyMealSlots];

        for (var dayIndex = 0; dayIndex < DaysInWeek; dayIndex++)
        {
            schedules[dayIndex] = BreakfastDefaultSchedule;
            schedules[DaysInWeek + dayIndex] = LunchDefaultSchedule;
            schedules[(2 * DaysInWeek) + dayIndex] = DinnerDefaultSchedule;
        }

        return schedules;
    }

    public static string SerializeDeliverySchedules(string[]? schedules)
    {
        return string.Join(DeliveryScheduleSeparator, NormalizeDeliverySchedules(schedules));
    }

    public static string[] ParseDeliverySchedules(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return CreateDefaultDeliverySchedules();

        return NormalizeDeliverySchedules(value.Split(DeliveryScheduleSeparator, StringSplitOptions.None));
    }

    private static string[] NormalizeDeliverySchedules(string[]? schedules)
    {
        if (schedules is null || schedules.Length == 0) return CreateDefaultDeliverySchedules();
        if (schedules.Length != WeeklyMealSlots) throw new ArgumentException("HorariosEntrega must contain exactly 21 delivery schedules.", nameof(HorariosEntrega));

        var normalizedSchedules = schedules
            .Select(schedule => schedule?.Trim() ?? string.Empty)
            .ToArray();

        for (var slotIndex = 0; slotIndex < normalizedSchedules.Length; slotIndex++)
        {
            ValidateDeliverySchedule(normalizedSchedules[slotIndex], slotIndex);
        }

        return normalizedSchedules;
    }

    private static void ValidateDeliverySchedule(string schedule, int slotIndex)
    {
        if (string.IsNullOrWhiteSpace(schedule)) return;

        var parts = schedule.Split('-', StringSplitOptions.TrimEntries);
        if (parts.Length != 2)
        {
            throw new ArgumentException("Delivery schedules must use HH:mm-HH:mm format.", nameof(HorariosEntrega));
        }

        if (!TimeOnly.TryParseExact(parts[0], "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var startTime)
            || !TimeOnly.TryParseExact(parts[1], "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var endTime))
        {
            throw new ArgumentException("Delivery schedules must use HH:mm-HH:mm format.", nameof(HorariosEntrega));
        }

        if (startTime >= endTime)
        {
            throw new ArgumentException("Delivery schedule end time must be after start time.", nameof(HorariosEntrega));
        }

        var mealTypeIndex = slotIndex / DaysInWeek;
        var minTime = mealTypeIndex == 0 ? new TimeOnly(5, 0) : new TimeOnly(12, 0);
        var maxTime = mealTypeIndex == 0 ? new TimeOnly(12, 0) : new TimeOnly(23, 0);

        if (startTime < minTime || endTime > maxTime)
        {
            throw new ArgumentException("Delivery schedule is outside the allowed window for that meal type.", nameof(HorariosEntrega));
        }
    }
}
