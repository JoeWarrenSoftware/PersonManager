namespace UKParliament.CodeTest.Web.Contracts.Responses;

public class PersonsResponse
{
    public required IEnumerable<PersonResponse> Items { get; init; } = Enumerable.Empty<PersonResponse>();
    public int TotalCount => Items.Count();
}