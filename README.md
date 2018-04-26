# PlogBot
The purpose of this project is to provide the clan "Ploggystyle" in the game Blade and Soul with clan logging and an interactive Discord bot that can associate discord accounts with characters, set alerts for clan events, and provide some entertaining experiences.

### Technology Stack

The project is written using C# on top of .NET Core 2.0.

##### Non-Microsoft Dependencies

| Name            | Version | Replacement Plan                                             |
| --------------- | ------- | ------------------------------------------------------------ |
| HtmlAgilityPack | 1.8.1   | Write own class to parse HTML efficiently using XML classes and XPath |
| Newtonsoft.Json | 11.0.2  | None, it's a dependable dependency                           |

### Data Persistence
PlogBot.Data defines a database context using Entity Framework Core.  Currently, the database is SQLite. This decision was made due to the limited scope of the project, and the ease of using SQLite.
#### Defined Entities
ClanMember - General information about a blade and soul character in Ploggystyle. Also has an optional Discord UserID associated.
ClanMemberStatLog - Snapshot in time of a clan member. Need to record this continuously and persist, since Blade and Soul APIs are not so good (slow, have to scrape some data off web pages.) There is also a need for fast querying, so a background process will populate these logs.

### Discord Bot
#### Functionality
- Add Blade and Soul Characters to the database through the Discord interface
  - Associate characters with a discord id
  - Query blade and soul characters when providing a discord user id
  - Administrator (Just me, all-powerful Leo) access to disassociate any clan members from their discord user ids
  - Add alts that are linked to your main blade and soul character
    - Query a discord user's alts
- Get lists of power rankings (top 10)
  - Whales, calculated from the clan stat logs.  Will use most recent for this calculation
  - Toxic players, calculated by listening for key phrases and adding toxicity points
    - For really toxic players, there might be a warning
- Voice functionality (discord bot will pop into the channel and say something quickly and leave)
  - Geoff saying, "That's TOXIC!"
  - Kash saying, "Mans not hot"
  - Sugartits saying things I can't repeat on my professional GitHub page
  - more...

### Logging
PlogBot.DataSync provides a program that will run every hour to populate the logs.
