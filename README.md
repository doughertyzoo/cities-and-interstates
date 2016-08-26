# cities-and-interstates
Your solution must accept an input file via command line parameter. Each line of this input file will be formatted like so:

<Population, in hundreds of thousands>|<City>|<State>|<Semicolon-delimited list of interstates that run through this city>\n

Example:

4|Raleigh|North Carolina|I-40;I-85;I-95
27|Chicago|Illinois|I-94;I-90;I-88;I-57;I-55
10|San Jose|California|I-5;I-80

You may assume the following:
-	The input file is well-formed: each pipe-delimited section will have one or more characters; the interstates section will have at least one interstate; the population number will be an integer; etc.
-	All interstates have the “I-” prefix.
-	The same city will not appear more than once in the input file.
-	Chicago will be in the input file.

Accompanying these instructions is a file named Sample_Cities.txt that you are encouraged to use to help construct your solution. 

Produce two output files from the input. The first must be named Cities_By_Population.txt and have data in the following format:

<Population>
(newline)
<City>, <State>
Interstates: <Comma-separated list of interstates, sorted by interstate number ascending>
(newline)

Cities must be ordered from highest population to lowest. If there are multiple cities with the same population, group them under a single <Population> heading and sort them alphabetically by state and then city.

Example output:

83

New York, New York
Interstates: I-78, I-80, I-87, I-95

27

Chicago, Illinois
Interstates: I-55, I-57, I-88, I-90, I-94

15

Phoenix, Arizona
Interstates: I-8, I-10, I-17

Philadelphia, Pennsylvania
Interstates: I-76, I-95 

The second output file must be named Interstates_By_City.txt and contain a list of interstates and the number of cities they run through. Each line of the output file must be of the form:

<Interstate> <Number of cities>

Sort the list by interstate number ascending.
Example output:

I-5 5
I-10 4
I-19 1
I-20 3 

