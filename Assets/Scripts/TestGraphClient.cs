using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using GraphQL;
using System;
using System.Net.Http;
using GraphQL.Query.Builder;


public class TestGraphClient : MonoBehaviour {
    private string POST_URL = "https://vbc2qnto3za4nare6aa2eygsfu.appsync-api.us-east-1.amazonaws.com/graphql";
    public TextMeshProUGUI text_dialog;

    async void Start() {
        try {
            // https://github.com/charlesdevandiere/graphql-query-builder-dotnet/tree/master
            IQuery<ResponseType> query = new Query<ResponseType>("query test")
            // .AddArguments(new { id = "xxxxxooooo", name = "123..." })
            .AddField(
                x => x.listTodos,
                y => y.AddField(
                    z => z.items,
                    a => a.AddField(n => n.name)
                          .AddField(n => n.id)
                )
            );
            var queryString = query.Build();
            Debug.Log(queryString);
            text_dialog.text = queryString;

            // query test{listTodos{items{name id}}}

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("x-api-key", "da2-q6xouvrjf5dr7aqpnwh4kmorqa");

            var options = new GraphQLHttpClientOptions {
                EndPoint = new Uri(POST_URL)
            };
            var graphQLClient = new GraphQLHttpClient(options, new NewtonsoftJsonSerializer(), httpClient);

            /*var request = new GraphQLRequest {
                Query = @"
                    query test {
                        listTodos {
                            items {
                                id
                                name
                            }
                        }
                    }
                "
            };*/
            
            var request = new GraphQLRequest(queryString);
            var graphQLResponse = await graphQLClient.SendQueryAsync<ResponseType>(request);
            List<Item> items = graphQLResponse.Data.listTodos.items;
            string result = "";
            foreach (Item item in items) {
                result = result + item.id + "   " + item.name + "\n";
            }
            text_dialog.text = text_dialog.text + "\n" + result;

        } catch (Exception e) {
            text_dialog.text = text_dialog.text + "\n" + e.Message;
        }
    }
}

public class ResponseType {
    public ListTodos listTodos { get; set; }
}

public class ListTodos {
    public List<Item> items { get; set; }
}

public class Item {
    public string id { get; set; }
    public string name { get; set; }
}
