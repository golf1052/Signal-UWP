# Signal-UWP

Even though this project is called Signal, it is *by no means an official Whispersystems project*.
It is intended to become a working Signal-like client for UWP (Universal Windows Platform).

## What to expect
* Nothing

## Subprojects
* [curve25519](https://github.com/golf1052/curve25519-pcl) - by langboost
* [libsignal-protocol-pcl](https://github.com/golf1052/libsignal-protocol-pcl) - C# port of libsignal-protocol-java
* [libsignalservice-uwp](https://github.com/golf1052/libsignalservice-uwp) - C# port of libsignal-service-java
* libsettings: A library for UserControls like a simpler UI for settings
* SignalTask: Background processes needed for WNS push
* Signal: The app
* SignalTest: Unit tests for libsignal-protocol-pcl

## Things To Know
Install [SQLite for UWP](https://marketplace.visualstudio.com/items?itemName=SQLiteDevelopmentTeam.SQLiteforUniversalWindowsPlatform) then reference it on the Signal project (under Universal Windows -> Extensions)  
Clone [libsignalservice-uwp](https://github.com/golf1052/libsignalservice-uwp) as well because the solution references that project.

### Original developer
* smndtrl (xmpp:simon@ssl.tophostingteam.de)
