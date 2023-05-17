# REST API Guide

![Rest Api Guide](https://dev-to-uploads.s3.amazonaws.com/i/hum1vpykx40t3hi24w4k.png)

## tl;dr

This is a guide with the goal of laying down foundational knowledge that is required when speaking about building REST API's. The following topics are covered:

- REST Constraints
- Richardson Maturity Model
- REST in Practice (Some practical guidelines)
- Example project (Written in C# using .NET 5) called _[Ranker]_

The main points that I would like to summarize with regards to REST are listed as follows:

- REST **IS** an architectural style used to describe web architecture
- REST **IS** protocol agnostic
- REST **IS** about web architecture (REST != API)
- REST **IS NOT** a design pattern
- REST **IS NOT** a standard. However standards can be used to implement REST.

## 1. REST Fundamentals

This sections covers REST essentials. The goal of this section is to make the reader comfortable with the notion of REST. It is also intended to provide the minimum required theory to start talking about REST and building HTTP services that incorporate a REST architectural style.

### Introduction

_REST (REpresentational State Transfer)_ is an architectural style that was defined by _[Roy Thomas Fielding]_ in his PhD dissertation _"[Architectural Styles and the Design of Network-based Software Architectures]"_.

According to _Fielding_,

> The name “Representational State Transfer” is intended to evoke an image of how a well-designed Web application behaves: a network of web pages (a virtual state-machine), where the user progresses through the application by selecting links (state transitions), resulting in the next page (representing the next state of the application) being transferred to the user and rendered for their use.
>
> (Fielding, 2000) pg 109

### Why REST?

If you're someone that builds HTTP services for distributed systems, then understanding and applying REST principles will help you build services that are more:

- scalable
- reliable
- flexible
- portable

By building services based on REST principles, one is effectively building services that are more web friendly. This is because REST is an architectural style that describes web architecture.

### REST Architectural Constraints

_Fielding_ describes REST as a hybrid style that is derived from several network-based architectural styles ([Chapter 3](https://www.ics.uci.edu/~fielding/pubs/dissertation/net_arch_styles.htm)) combined with a number of additional constraints. In this section, the six architectural constraints as applied to REST are discussed. The key takeaway is that these constraints encourage design that will result in applications that easily scale, are faster, and more reliable.

The 6 architectural REST constraints are as follows:

1\. Client-Server

  A guiding principle of _[Client-Server]_ is the _[separation of concerns]_. It's all about achieving high cohesion and loose coupling in order to improve portability and flexibility. It also allows systems to evolve independently of each other. As can be seen by the diagram below, a _Client_ sends a request, and a _Server_ receives the request.

  ![rest-client-server](https://user-images.githubusercontent.com/33935506/72971024-ba673d80-3e2d-11ea-963b-389138d424d5.png)

2\. Statelessness

  A _Server_ must not store any state during communications. All information required to understand a request must be contained within the _Request_. Therefore, every _Request_ should be able to execute on its own and be self-contained. Also, a _Client_ must maintain it's own state. The benefit of this approach is as follows:

  - Visibility - Everything required to understand the _Request_ is within the _Request_. This makes monitoring a request easier.
  - Reliability - Recovering from failures is easier because the _Server_ does not need to track/rollback/commit state because all the state is essentially captured within 
    the message. If a _Request_ fails, it can be as simple as resending the _Request_.
  - Scalability - Because there is no need to manage state and resources between requests, and because all _Requests_ are isolated,scalability is improved and simplified.
  - Aligned with web architecture (the internet is designed this way)

  ![rest-stateless](https://user-images.githubusercontent.com/33935506/72973703-9bb77580-3e32-11ea-9f3d-beba73f1b324.png)

  A disadvantage of this approach is that it decreases network efficiency because the _Requests_ need to contain all the information required for that interaction. The more information, the larger the _Request_ size, and therefore the more bandwidth is used. This will have a negative effect on latency as well.

3\. Cache

  The primary reason for the _Cache_ constraint is to improve network efficiency. As noted above in the _Stateless_ constraint, the size of _Requests_ can decrease network efficiency due to the need for more bandwidth. Through caching, it is possible to reduce and sometimes remove the need for a _Client_ to interact with the _Server_. In other words it's possible to reduce and/or eliminate the need for _Requests_. Therefore, the _Cache_ constraint states that a _Server_ must include additional data in the response to indicate to the client whether the _Request_ is cacheable and for how long. A network _Client_ can then decide the appropriate action based on provided cache information in _Response_.

  Caching can improve performance. However, it comes with a number of disadvantages that impact the reliability of the system. For example:

  - Data Integrity - Response data could be inaccurate due to stale or expired data
  - Complexity - The implementation and use of caching mechanisms is renowned for it's complexity in the _Computer Science_ world

4\. Uniform Interface

  At the core of this constraint is the _principle of generality_ which is closely related to the _principle of anticipation_. It stems from the fact that it is impossible to build the exact required interface for all network clients of a server service. Therefore, by providing a generic interface, one is able to provide a simplified interface with higher visibility that is able to satisfy the requirements of more clients. A disadvantage of this approach is that because the interface is so general, one is not able to satisfy specific client requirements. In other words, providing a generic interface can lead to a sub-optimal interface for many clients.

  There are four additional constraints that form part of the _Uniform Interface_ and are listed as follows:

  - Identification of resources

    A key abstraction of REST is a resource. According to _Fielding_ ([Resources and Resource Identifiers](https://www.ics.uci.edu/~fielding/pubs/dissertation/rest_arch_style.htm#sec_5_2b)), a resource is any information that can be named. Furthermore, I personally like to think of a resource as a _"Noun"_.

    > Noun - a word (other than a pronoun) used to identify any of a class of people, places, or things ( common noun ), or to name a particular one of these ( proper noun ).

    It is also better to think of a single resource as a collection of resources. For example, if we were to provide an API to allow a _Client_ to submit or retrieve a "rating", one would typically identify the resource as follows:

    ```javascript
    GET /ratings
    ```

    Generally, there should only be a single way to access a resource. But this is more a guideline than a rule.

  - Manipulation of resources through representations
    
    This constraint states that the client should hold the representation of a resource that has enough information to create, modify or delete a resource. It's important that the representation of a resource is decoupled from the way the resource is identified. A resource can be represented in multiple formats or representations such as JSON, XML, HTML, PNG etc. A client should be able to specify the desired representation of a resource for any interaction with the server. Therefore, a _Client_ can specify to receive a resource in _JSON_ format, but send the resource as input in _XML_ format.

    For example:

    For the retrieval of an Employees resource, we use XML format
    by specifying a "Accept: application/xml" header.

    ```java
    GET /ratings
    Accept: application/xml

    <ratings>
      <rating>
        <id>7337</id>
        <userId>98765</userId>
        <movieId>12345</movieId>
        <score>6</score>
      </rating>
    </ratings>
    ```
    
    For the creation of an Employees resource, we use
    JSON format by specifying a "Content-Type: application/json" header

    ```javascript
    POST /ratings
    Content-Type: application/json
    {
        "userId": 98765,
        "movieId": 12345,
        "score": 6
    }     
    ```
    
    Should a specific format not be supported, it is important for the _Server_ to provide an appropriate response to indicate that a specific format is not supported. For example:

    - Return a __406 Not Acceptable__ status code to indicate that the client specified a request with an _Accept_ header format that the _Server_ is unable to fulfill. [See here for more information]
    - Return a __415 Unsupported Media Type__ when a response is specified in an unsupported content type. [See here for more information]

  - Self descriptive messages
    
    Self descriptive messages enable intermediary communication by allowing intermediary components to transform the content of the message. In other words, the semantics of the message are exposed to the intermediaries. The implication of this constraint is that interactions are stateless, standard methods and media types are used to expose the semantics of message, and responses indicate cacheability.

  - Hypermedia as the engine of application state (HATEOAS)
    
    A key concept about HATEOAS is that it implies that a _Response_ sent from a _Server_ should include information that informs the _Client_ on how to interact with the _Server_.
    
    ![rest-hateoas](https://user-images.githubusercontent.com/33935506/73115137-2d37fc00-3f87-11ea-925c-9ca21ba1e41c.png)
    
    The advantages of _HATEOAS_ are as follows:

    - Improves discoverability of resources through published set of links (provided with response)

    - Indicates to _Clients_ what actions can be taken next. In other words, without _HATEOAS_, a _Client_ only has access to the data but no idea about what actions may be taken with that data

5\. Layered System

  The key principle of this constraint is that the _Client_ cannot make any assumptions that it is communicating directly with the _Server_. This constraint relates to the _Client-Server_ constraint (discussed above) in such a way that _Client_ and _Server_ are decoupled. Therefore the _Client_ makes no assumptions about any kind of hidden dependencies and this enables us to insert components and entire sub-systems between the _Client_ and the _Server_. This allows one to add load balancers, DNS, caching servers and security (authentication and authorization) between _Client_ and _Server_ without disrupting the interaction.

  Layering allows one to evolve and improve ones architecture to improve scalability and reliability ones system.

6\. Code On Demand

  This is an optional constraint. The key concept about this constraint is that when a _Client_ makes a request to a resource on a _Server_, it will receive the resource as well as the code to execute against that resource. The _Client_ knows nothing about the composition of the code and only needs to know how to execute it. Javascript is an example of where this is done.

### Richardson Maturity Model

The _[Richardson Maturity Model] (RMM)_ is a heuristic maturity model that can be used to better understand how mature a service is in terms of the REST architectural style.

![rest-rmm](https://user-images.githubusercontent.com/33935506/73115617-7a1ed100-3f8d-11ea-87ed-7591e3be8b13.png)

- Level 0
  
  Services at this level are described as having a single URI, and using a single HTTP verb (usually POST). This is very characteristic of most Web Services (WS-*) in that this services would have a single URI accepting an HTTP POST request having an XML payload.

- Level 1
  
  Services at this level are described as having many URIs with a single HTTP verb. The primary difference between Level 0 and Level 1 is that Level 1 services expose multiple logical resources as opposed to a single resource.

- Level 2
  
  Services at this level are described as having many URI-addressable resources. Each addressable resource supports both multiple HTTP verbs and HTTP status codes.

- Level 3
  
  Services at this level are like _Level 2_ services that additionally support _Hypermedia As The Engine Of Application State (HATEOAS)_. Therefore, representations of a resource will also contain links to other resources (the actions that can be performed relating to current resource).

When thinking about the _RMM_ applies to your API, please refrain from thinking in terms of having a _Level 2_ or _Level 3_ REST API. According to this model, an API cannot be called a REST API unless it at least satisfies a Level 3 of the _RMM_. Therefore, it would be better to think of ones API as an HTTP API that satisfies a _Level 1,2, or 3_ on the _RMM_.

---

## 2. REST in Practice

I have developed a simple Http Api to demonstrate some of the concepts that I discussed in Part 1 of this guide.

- [Ranker]
  
  > A REST API guide with and example project written in C# using .NET 5

I've also started another project that I plan to use to demonstrate various technology concepts like REST API's.

- [Chinook]
  
  > A playground for demonstrating concepts such as architecture, design, dotnet core, typescript, react, database and docker

### 2. Defining A Contract

In this example, we are going to define contracts for 3 types of resources:

- Users
- Movies
- Ratings

There are 5 important aspects to defining a contract:

- Naming a resource
- Http methods used to interact with resource
- Status codes used to describe the state of an interaction
- Content Negotiation
- Be consistent

#### 2.1 Naming Guidelines

- Resources should have names that are represented by nouns and not actions (behaviors)

  ```yaml
  # Incorrect naming

  /getUsers
  /getUserById/{userId}

  # Correct Naming

  /users
  /users/{userId}
  ```

- Resources should be named using plural form

  ```yaml
  # Incorrect naming

  /user
  /movie
  /rating

  # Correct naming

  /users
  /movies
  /ratings
  ```

- Mapping RPC style methods to resources

  The naming guidelines seem to suit naming resources very well. However, what happens when one needs to name something that is more a behavior than a resource? For example, let's say we want to compute the average rating for a movie. How would we structure our naming?

  ```yaml
  /movies/{movieId}/averageRating
  ```

  I don't think there is 100% consensus on what the correct naming strategy is for a scenario such as this one. However, when faced with defining a contract for something that feels more about behavior than resources, I like to define contracts based on the outcomes of those behaviors. Therefore, for the example above:

  ```yaml
  /averageMovieRatings
  /averageMovieRatings/{movieId}
  ```

  But what if we try to define a contract for a calculator? This is clearly an example of where defining a contract around a behavior is very difficult and "unnatural" to REST. The reason why it feels unnatural is because REST is an architectural style for describing web architecture. So if you imagined every endpoint as a webpage, then clearly the behaviors for a calculator don't map very well. My suggestion is to use an alternative technology like gRPC if you are building API's that are more about behavior than resources.

- Represent hierarchy
  
  ```yaml
  /users/{userId}
  /users/{userId}/ratings
  /users/{userId}/ratings/{ratingId}

  /movies/{movieId}
  /movies/{movieId}/ratings
  /movies/{movieId}/ratings/{ratingId}
  ```

- Filtering, searching and sorting are not part of naming
  
  For filtering:

  ```yaml
  # Incorrect
  /users/firstName/{firstName}

  # Correct
  /users?firstName={firstName}
  ```

  For searching:

  ```yaml
  # Incorrect
  /users/search/{query}

  # Correct
  /users?q={query}
  ```

  For ordering:

  ```yaml
  # Incorrect
  /users/orderBy/{firstName}

  # Correct
  /users?order={firstName}
  ```

#### 2.2 Http Methods

| Http Method | Request Body                 | Uri             | Response      |
|-------------|------------------------------|-----------------|---------------|
| GET         | -                            | /users          | List of users |
| GET         | -                            | /users/{userId} | Single user   |
| POST        | Single user                  | /users          | Single user   |
| PUT         | Single user                  | /users/{userId} | -             |
| PATCH       | Json Patch Document for user | /users/{userId} | -             |
| DELETE      | -                            | /users/{userId} | -             |
| HEAD        | -                            | /users          | -             |
| HEAD        | -                            | /users/{userId} | -             |
| OPTIONS     | -                            | /users          | -             |

#### 2.3 Status Codes

In this section, a list of commonly used status codes is provided. Status codes help convey meaning in client/server interactions. They also help achieve consistency in terms of defining a contract.

Level 200 - Success

- [200 Ok] - Request succeeded
- [201 Create] - Request succeeded and resource created
- [204 No Content] - Request succeeded and there is no additional content to send in response body

Level 300 - Redirection Responses

- [301 Moved Permanently] - The URL of requested resourced has changed permanently. The new URL is provided in response
- [302 Found] - Indicates that the URI of requested resource changed, and can therefore use the same URI for future requests
- [304 Not Modified] - Used for caching. Indicates that the resource has not changed and that the same cached version can be used

Level 400 - Client Mistake

- [400 Bad Request] - The request could not be understood by the server due to malformed syntax. The client should not repeat the request without modifications
- [401 Unauthorized] - Request failed due to authentication failure
- [403 Forbidden] - Request failed due to authorization failure
- [404 Not Found] - The requested resource could not be found
- [405 Method Not Allowed] - The request method is understood by server but not supported. In other words, the server doesn't have an endpoint supporting requested method.
- [406 Not Acceptable] - When a request is specified in an unsupported content type using the Accept header
- [409 Conflict] - Indicates a conflict in terms of requested resource state. For a POST, it could mean that a resource already exists. For a PUT, it could mean that the state of resource changed thereby making current request data stale.
- [415 Unsupported Media Type] - When a response is specified in an unsupported content type
- [422 Unprocessable Entity] - Indicates the the request was correct and understood by server, but the data contained within request is invalid.

Level 500 - Server Mistake

- [500 Internal Server Error] - Indicates that something went wrong on the server that prevent the server from fulfilling the request.
- [503 Service Unavailable] - Indicates that the server is functional but not able to deliver requested resource. This is usually a result of a server being overloaded, server is under maintenance, or a client side issue relating to DNS server (DND server could be unavailable).
- [504 Gateway Timeout] - Indicates that a proxy server did not receive a timely response from the origin (upstream) server.

#### 2.4 Content Negotiation

Implies the type of representation (Media Type) that will be used for request and response. The _Media Type_ is specified in header of request. Two popular Media Type formats that are used with Http Api's are:

- application/json
- application/xml

Typically, I would support at least the two aforementioned formats. For any media type format that is not supported, the Api should return a [406 Not Acceptable] status code.

Examples:

```yaml
# Send POST request to create a a new user.
# The request will use 'application/json' as input, but XML in return (application/xml)

POST /users
Accept: application/xml
Content-Type: application/json

{
  "firstName": "Bob",
  "lastName": "TheBuilder"
}

# The response is returned as XML

<User>
  <Id>112233</Id>
  <FirstName>Bob</FirstName>
  <LastName>TheBuilder</LastName>
</User>
```

---

## 3. Example Project

To illustrate some of the topics that have been discussed, I created an example project called _[Ranker]_.

_[Ranker]_ is an API that has been designed by using REST as a guide. In terms of the [Richardson Maturity Model], I have implemented all endpoints to be at least a _Level 2_. However, I have implemented some endpoints to a _Level 3_ (with HATEOAS). Conceptually, _[Ranker]_ provides the following features:

- Interface to manage _Users_ (with HATEOAS)
- Interface to manage _Movies_
- Interface to manage _Ratings_

In the following sections I provide more detail about the project and how to get started.

### Architecture

Although the focus of this example project is to illustrate an implementation of REST, I decided to provide a basic architecture to also illustrate a good separation of concerns so that the Api layer (Controllers) are kept very clean.

I've chosen a architecture based on the [Onion Architecture]. Below, I provide 2 different views of what equates to exactly the same architecture.

**Layered Architecture**

<p align="center">
  <img alt="Layered Architecture" src="https://user-images.githubusercontent.com/33935506/73599890-05135300-45ae-11ea-8f0f-2b684763235b.png" />
</p>

**Onion Architecture**

  <p align="center">  
    <img alt="Onion Architecture" src="https://user-images.githubusercontent.com/33935506/73599926-7ce17d80-45ae-11ea-80f7-93181e65c86e.png" />
  </p>

- API
  
  Primary Responsibility: Provides a distributed interface that gives access to application features

  This API has been implemented as a number of HTTP services based on REST guidelines. The API itself is based on an MVC (Model, View, and Controllers) architecture. The _Controllers_ are essentially the public facing API contract.

- Infrastructure
  
  Primary Responsibility: Provide the core of the system an interface to the "world".

  This layer is all about defining and configuring external dependencies such as:
  - database access
  - proxies to other API's
  - logging
  - monitoring
  - dependency injection

- Application

  Primary Responsibility: Application logic.

  This layer is typically where you would find "Application "Services".  

- Domain
  
  Primary Responsibility: Enterprise domain logic.

  All domain logic relating to domain models and domain services are handled in this layer.

### API Contract

The API has been implemented with the Open Api Specification (OAS). Once you have the API up and running, you can browse to the following Url to get access to the OAS Swagger Document.

```javascript
http://localhost:5000
```

The Swagger document will look something like below:

![movies-oas](https://user-images.githubusercontent.com/33935506/73600055-1e1d0380-45b0-11ea-8e0c-43bf714f10e5.png)

![ratings-oas](https://user-images.githubusercontent.com/33935506/73600054-1e1d0380-45b0-11ea-8f7d-bde636b3b7f9.png)

![users-oas](https://user-images.githubusercontent.com/33935506/73600053-1d846d00-45b0-11ea-8986-472394f0d23c.png)

### Pagination

For this project, any endpoint returning a collection of items has been implemented with paging. Use the following query parameters to control paging:

- page - the page number
- limit - the number of items per page

Pagination has been implemented in two ways for this example project.

- Pagination in Header
  
  ```javascript
  GET http://localhost:5000/v1/movies?page=2&limit=5

  Header: X-Pagination

  {
      "CurrentPageNumber": 2,
      "ItemCount": 9742,
      "PageSize": 5,
      "PageCount": 1949,
      "FirstPageUrl": "http://localhost:5000/v1/movies?page=1&limit=5",
      "LastPageUrl": "http://localhost:5000/v1/movies?page=1949&limit=5",
      "NextPageUrl": "http://localhost:5000/v1/movies?page=3&limit=5",
      "PreviousPageUrl": "http://localhost:5000/v1/movies?page=1&limit=5",
      "CurrentPageUrl": "http://localhost:5000/v1/movies?page=2&limit=5"
  }
  ```

- Pagination as links (HATEOAS)
  
  ```javascript
  GET http://localhost:5000/v1/users?page=1&limit=1

  {
    .
    .
    .
    "links": [
        {
            "href": "http://localhost:5000/v1/users?page=1&limit=1",
            "method": "GET",
            "rel": "current-page"
        },
        {
            "href": "http://localhost:5000/v1/users?page=2&limit=1",
            "method": "GET",
            "rel": "next-page"
        },
        {
            "href": "",
            "method": "GET",
            "rel": "previous-page"
        },
        {
            "href": "http://localhost:5000/v1/users?page=1&limit=1",
            "method": "GET",
            "rel": "first-page"
        },
        {
            "href": "http://localhost:5000/v1/users?page=610&limit=1",
            "method": "GET",
            "rel": "last-page"
        }
    ]
  }
  ```

### Filtering

Where practical, I've tried to provide a filter per resource property. I've implemented filtering using 3 techniques:

1\. Basic

   ```javascript
   // filter users by last name and age

   GET http://localhost:5000/v1/users?last-name=doe&gender=male
   ```

2\. Range

   For numeric resource (and date) properties, I've implemented range filters as follows:

   ```javascript
   // Possible input for age could be:
   // age=gt:30
   // age=gte:30
   // age=eq:30
   // age=lt:30
   // age=lte:30
   
   GET http://localhost:5000/v1/users?age=gte:30
   ```

3\. Multiple (comma separated values)

   ```javascript
   // get a list of movies for the genres animation and sci-fi

   GET http://localhost:5000/v1/movies?genres=animation,sci-fi
   ```

### Ordering

I've chosen to keep _ordering_ parameters very succinct. Therefore, ordering for a collection of resources may be executed in the following ways:

- Order by a single resource property in ascending order
  
  ```javascript
  // order by last name ascending

  GET http://localhost:5000/v1/users?order=last-name
  ```

- Order by a single resource property in descending order
  
  ```javascript
  // order by age descending

  GET http://localhost:5000/v1/users?order=-age
  ```

- Order by multiple resource properties using mixed sort orders
  
  Notice that we use comma separated values for the order.

  ```javascript
  // order by last-name ascending then by age descending

  GET http://localhost:5000/v1/users?order=last-name,-age
  ```  

### Caching

I have implemented some basic client side caching behavior.

For example:

The following endpoints use response caching where the cache expires after 10 seconds.

```javascript
GET http://localhost:5000/v1/users

GET http://localhost:5000/v1/movies
GET http://localhost:5000/v1/movies/{movieId}

GET http://localhost:5000/v1/ratings
GET http://localhost:5000/v1/ratings/{ratingId}
```

The following endpoint uses caching with an ETag.

```javascript
GET http://localhost:5000/v1/users/{userId}
```

### HATEOAS

The following endpoints have been implemented to return links as part of response.

```javascript
// Get links available from root

GET http://localhost:5000/v1

[
    {
        "href": "http://localhost:5000/v1",
        "method": "GET",
        "rel": "self"
    },
    {
        "href": "http://localhost:5000/v1/movies",
        "method": "GET",
        "rel": "movies"
    },
    {
        "href": "http://localhost:5000/v1/movies",
        "method": "POST",
        "rel": "create-movie"
    },
    {
        "href": "http://localhost:5000/v1/ratings",
        "method": "GET",
        "rel": "ratings"
    },
    {
        "href": "http://localhost:5000/v1/ratings",
        "method": "POST",
        "rel": "create-rating"
    },
    {
        "href": "http://localhost:5000/v1/users",
        "method": "GET",
        "rel": "users"
    },
    {
        "href": "http://localhost:5000/v1/users",
        "method": "POST",
        "rel": "create-user"
    }
]
```

```javascript
// Get as single user, including a list of navigational links

GET http://localhost:5000/v1/users

{
    "userId": 10,
    "age": 30,
    "firstName": "Durham",
    "lastName": "Franks",
    "gender": "male",
    "email": "durhamfranks@kog.com",
    "links": [
        {
            "href": "http://localhost:5000/v1/users/10",
            "method": "DELETE",
            "rel": "delete-user"
        },
        {
            "href": "http://localhost:5000/v1/users/10",
            "method": "GET",
            "rel": "self"
        },
        {
            "href": "http://localhost:5000/v1/users?Page=1&Limit=10",
            "method": "GET",
            "rel": "users"
        },
        {
            "href": "http://localhost:5000/v1/users",
            "method": "OPTIONS",
            "rel": "options"
        },
        {
            "href": "http://localhost:5000/v1/users/10",
            "method": "PATCH",
            "rel": "patch-user"
        },
        {
            "href": "http://localhost:5000/v1/users",
            "method": "POST",
            "rel": "create-user"
        },
        {
            "href": "http://localhost:5000/v1/users/10",
            "method": "PUT",
            "rel": "update-user"
        },
        {
            "href": "http://localhost:5000/v1/users/10/ratings",
            "method": "GET",
            "rel": "ratings"
        }
    ]
}
```

And for a collection of users (with links), we can use the request below. Please take note of the paging information that is returned as part of response

```javascript
// Get list of users (with links), and paging links

GET http://localhost:5000/v1/users

{
    "users": [
        {
            "userId": 23,
            "age": 40,
            "firstName": "Michele",
            "lastName": "Jacobs",
            "gender": "female",
            "email": "michelejacobs@kineticut.com",
            "links": [
                {
                    "href": "http://localhost:5000/v1/users/23",
                    "method": "DELETE",
                    "rel": "delete-user"
                },
                {
                    "href": "http://localhost:5000/v1/users/23",
                    "method": "GET",
                    "rel": "self"
                },
                {
                    "href": "http://localhost:5000/v1/users?Page=1&Limit=10",
                    "method": "GET",
                    "rel": "users"
                },
                {
                    "href": "http://localhost:5000/v1/users",
                    "method": "OPTIONS",
                    "rel": "options"
                },
                {
                    "href": "http://localhost:5000/v1/users/23",
                    "method": "PATCH",
                    "rel": "patch-user"
                },
                {
                    "href": "http://localhost:5000/v1/users",
                    "method": "POST",
                    "rel": "create-user"
                },
                {
                    "href": "http://localhost:5000/v1/users/23",
                    "method": "PUT",
                    "rel": "update-user"
                },
                {
                    "href": "http://localhost:5000/v1/users/23/ratings",
                    "method": "GET",
                    "rel": "ratings"
                }
            ]
        },
        {
            "userId": 33,
            "age": 40,
            "firstName": "Barnett",
            "lastName": "Griffith",
            "gender": "male",
            "email": "barnettgriffith@corpulse.com",
            "links": [
                {
                    "href": "http://localhost:5000/v1/users/33",
                    "method": "DELETE",
                    "rel": "delete-user"
                },
                {
                    "href": "http://localhost:5000/v1/users/33",
                    "method": "GET",
                    "rel": "self"
                },
                {
                    "href": "http://localhost:5000/v1/users?Page=1&Limit=10",
                    "method": "GET",
                    "rel": "users"
                },
                {
                    "href": "http://localhost:5000/v1/users",
                    "method": "OPTIONS",
                    "rel": "options"
                },
                {
                    "href": "http://localhost:5000/v1/users/33",
                    "method": "PATCH",
                    "rel": "patch-user"
                },
                {
                    "href": "http://localhost:5000/v1/users",
                    "method": "POST",
                    "rel": "create-user"
                },
                {
                    "href": "http://localhost:5000/v1/users/33",
                    "method": "PUT",
                    "rel": "update-user"
                },
                {
                    "href": "http://localhost:5000/v1/users/33/ratings",
                    "method": "GET",
                    "rel": "ratings"
                }
            ]
        }
    ],
    "links": [
        {
            "href": "http://localhost:5000/v1/users?order=-age&page=1&limit=2",
            "method": "GET",
            "rel": "current-page"
        },
        {
            "href": "http://localhost:5000/v1/users?order=-age&page=2&limit=2",
            "method": "GET",
            "rel": "next-page"
        },
        {
            "href": "",
            "method": "GET",
            "rel": "previous-page"
        },
        {
            "href": "http://localhost:5000/v1/users?order=-age&page=1&limit=2",
            "method": "GET",
            "rel": "first-page"
        },
        {
            "href": "http://localhost:5000/v1/users?order=-age&page=305&limit=2",
            "method": "GET",
            "rel": "last-page"
        }
    ]
}
```

## 4. Technology Used

### OS

I have developed and tested _Ranker_ on the following Operating Systems.

- [Ubuntu 18.04 LTS]

  Ubuntu is an open source software operating system that runs from the desktop, to the cloud, to all your internet connected things.

- Windows 10 Professional

  In addition to developing _Ranker_ on Windows 10, I have also tried and tested _Ranker_ using [Windows Subsystem For Linux]. Specifically, I have used [WSL-Ubuntu]. See more about [WSL] below.

  - [Windows Subsystem For Linux]

    The Windows Subsystem for Linux lets developers run a GNU/Linux environment -- including most command-line tools, utilities, and applications -- directly on Windows, unmodified, without the overhead of a virtual machine.

  - [Windows Subsystem For Linux 2]
  
    NOTE: I have not tested _Ranker_ on [WSL2] yet. I mention it here because I want to be clear that I've only tested on [WSL] (not to be confused with WSL2).

    WSL 2 is a new version of the architecture in WSL that changes how Linux distros interact with Windows. WSL 2 has the primary goals of increasing file system performance and adding full system call compatibility. Each Linux distro can run as a WSL 1, or a WSL 2 distro and can be switched between at any time. WSL 2 is a major overhaul of the underlying architecture and uses virtualization technology and a Linux kernel to enable its new features.

### Code

- [Visual Studio Code]

  Visual Studio Code is a source code editor developed by Microsoft for Windows, Linux and macOS. It includes support for debugging, embedded Git control, syntax highlighting, intelligent code completion, snippets, and code refactoring.

- [Visual Studio Community Edition]

  A fully-featured, extensible, **FREE** IDE for creating modern applications for Android, iOS, Windows, as well as web applications and cloud services.

### Database

- Kept things simple and only used an in-memory database

---

## 5. Getting Started

Before getting started, the following frameworks must be installed on your machine:

- Dotnet Core 3.1

### Get The Code

Clone 'ranker' repository from GitHub

```bash
# using https
git clone https://github.com/drminnaar/ranker.git

# or using ssh
git clone git@github.com:drminnaar/ranker.git
```

### Build The Code

```bash
# change to project root
cd ./ranker

# build solution
dotnet build
```

### Running the API

Run the API from the command line as follows:

```bash
# change to project root
cd ./ranker/Ranker.Api

# To run 'Ranker Api' (http://localhost:5000)
dotnet watch run
```

### Open Postman Collection

I have provided a postman collection for the _Ranker_ API. Please find the Postman collection _'Ranker.postman_collection'_at the root of the solution.

![ranker-postman-collection](https://user-images.githubusercontent.com/33935506/73604546-5dbb0e00-45f7-11ea-9d3e-dcb8151bca82.png)

---

## 6. Versioning

I use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/drminnaar/ranker/tags).

- [V1.0.0 - .NET Core 3.1 Version](https://github.com/drminnaar/ranker/releases/tag/V1.0.0)

---

## 7. Authors

- **Douglas Minnaar** - *Initial work* - [drminnaar](https://github.com/drminnaar)

---

[Roy Thomas Fielding]: https://roy.gbiv.com/
[Architectural Styles and the Design of Network-based Software Architectures]: https://www.ics.uci.edu/~fielding/pubs/dissertation/fielding_dissertation.pdf
[separation of concerns]: https://en.wikipedia.org/wiki/Separation_of_concerns
[Client-Server]: https://www.ics.uci.edu/~fielding/pubs/dissertation/net_arch_styles.htm#sec_3_4_1
[Richardson Maturity Model]: https://www.crummy.com/writing/speaking/2008-QCon/act3.html
[Onion Architecture]: https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1/
[Entity Framework]: https://docs.microsoft.com/en-us/ef/core/
[Visual Studio Code]: https://code.visualstudio.com/
[VS Code]: https://code.visualstudio.com/
[Visual Studio Community Edition]: https://visualstudio.microsoft.com/vs/community/
[Ubuntu]: https://ubuntu.com/download/desktop
[Ubuntu 18.04]: https://ubuntu.com/download/desktop
[Ubuntu 18.04 LTS]: https://ubuntu.com/download/desktop
[Windows Subsystem For Linux]: https://docs.microsoft.com/en-us/windows/wsl/about
[WSL]: https://docs.microsoft.com/en-us/windows/wsl/about
[Windows Subsystem For Linux 2]: https://docs.microsoft.com/en-us/windows/wsl/wsl2-install
[WSL2]: https://docs.microsoft.com/en-us/windows/wsl/wsl2-install

[200 Ok]: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/200
[201 Create]: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/200
[204 No Content]: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/204
[301 Moved Permanently]: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/301
[302 Found]: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/302
[304 Not Modified]: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/304
[400 Bad Request]: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/400
[401 Unauthorized]: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/401
[403 Forbidden]: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/403
[404 Not Found]: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/404
[405 Method Not Allowed]: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/405
[406 Not Acceptable]: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/406
[409 Conflict]: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/409
[415 Unsupported Media Type]: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/415
[422 Unprocessable Entity]: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/422
[500 Internal Server Error]: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/422
[503 Service Unavailable]: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/503
[504 Gateway Timeout]: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/504

[Ranker]:https://github.com/drminnaar/ranker
[Chinook]:https://github.com/drminnaar/chinook
