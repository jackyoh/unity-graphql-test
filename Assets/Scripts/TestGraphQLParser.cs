using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphQL.Query.Builder;

public class TestGraphQLParser : MonoBehaviour {

    void Start() {
        IQuery<Human> query = new Query<Human>("humans")
          .AddArguments(new { id = "xxxxxx" })
          .AddField(h => h.FirstName)
          .AddField(h => h.LastName)
          .AddField(
            h => h.HomePlanet,
            sq => sq
                .AddField(p => p.Name)
          );
        Debug.Log("{" + query.Build() + "}");
    }
}

class Human {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Planet HomePlanet { get; set; }
    public IEnumerable<Human> Friends { get; set;}    
}

class Planet {
    public string Name { get; set; }
}
