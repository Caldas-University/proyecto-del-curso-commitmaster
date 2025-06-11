namespace EventLogistics.Application.Interfaces;

using EventLogistics.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IReasignacionServiceApp
{
    Task<List<Guid>> SearchEventAsync(Dictionary<string, object> criterios);
    Task<bool> DetectChangeAsync();
    Task<Dictionary<string, object>> AnalyzeRulesAsync();
    Task<bool> ReasignResourcesAsync(List<Guid> recursos, List<Guid> eventos);
    Task<bool> RescheduleActivitiesAsync(List<Guid> actividades);
    Task<Dictionary<string, double>> EvaluateImpactAsync();
    Task<bool> ManageConflictsAsync(List<Dictionary<string, object>> conflictos);
    Task<ReasignacionDto> CreateReasignacionAsync(string tipo);
}
