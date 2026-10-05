# Code review for *SwimmingPlayer* Class

Reviewer: Zhenxiang "ZX" Ning

Based on commit: 0f4c1ae5a49584e7a6440f4a2cfa7ef5c0f12953

## Readability: 9/10

Code is clear, well-structured, and easy to follow.
Some class fields, like *jumpTimer*, do not have very clear names.
Also, some comments/documents explaining how Dash methods are implemented would be better.

## Maintainability/Quality: 7.5/10

DashMaxSpeed const field is not used. Maybe remove it or explain future usage in the comments of Dash.
The SwimmingPlayer.cs file has 205 lines of code and contains a lot of the same logic as in Player.cs.
May consider refactoring the whole structure of Player in future development.
Considering the class itself, the implementation is overall good and functions as expected.