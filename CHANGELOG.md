# TaskProcessing Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).

## v1.3.6
### Added
- Action Task kann den ExecutionContext als Parameter entgegen nehmen
- ExecutionContext kann einen Subtask im selben Thread ausführen

## v1.3.5
### Added
- Option damit der ITaskServer im Builder von TaskServer.Setup() zugewiesen werden kann
- Option damit der IScheduler im Builder von TaskServer.Setup() zugewiesen werden kann

## v1.3.4
### Added
- Es kann geprüft werden ob ein Task Scheduled ist

## v1.3.3
### Added
- Es kann geprüft werden ob ein Task im Scheduler registriert ist

## v1.3.2
### Fixed
- Lock in TaskStore hinzugefügt

## v1.3.1
### Added
- TaskServer kann Tasks mit dem Resolver starten
- Der Name des Threads wird gesetzt

### Fixed
- Dispose wird auf den Tasks aufgerufen nachdem sie ausgeführt wurden

## v1.3.0
### Added
- WP-6637 Cron Expressions in TaskProcessor zulassen
- WP-6633 Tasks können aus dem Scheduler entfernt werden
- DebuggerDisplay für Schedules für einfacheres Debugging hinzugefügt

### Fixed
- WP-6633 TaskProcessing - DayUnit nimmt falsche Zeit wenn die Zeit in der Vergangenheit ist

## v1.2.2
### Fixed
- DayUnit startet immer erst am nächsten Tag

### Added
- GitLab CI

### Changed
- Update .Net Framework

## v1.2.1
### Fixed
- AndEvery() did not reschdule after a In()

## v1.2.0
### Fixed
- Schedule Every() now waits for the given duration before executing the first time

### Changed
- In() uses a timeunit now
- Simlified In() and At()

## v1.1.3
### Added
- End the Execution of a Task

## v1.1.2
### Added
- Write a log when a new task is enqueued

### Fixed
- Scheduler did nor recalculate schedule when a new entry was added

## v1.1.0
### Added
- Extension to execute a simple Action instead of IBackgroundTask

## v1.0.0
### Added
- WP-5513 Scheduler zum definieren von Zeiten erstellt
- Calculate TimeSpan to next execution
- Create a schedule at a certain time and day
- Readme.md in Package hinzugefügt
- Monitorobjekt mit Informationen zu den Schedules und Tasks
- Add function to logs

### Changed
- Set state of failing tasks to Error

### Fixed
- 

