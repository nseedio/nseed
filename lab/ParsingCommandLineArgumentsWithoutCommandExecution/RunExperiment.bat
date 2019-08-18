set run=dotnet run --project Experiment/Experiment.csproj --

%run%

%run% new -v

%run% info --verbose

%run% new -nc

%run% info --no-color

%run% new -v -nc

%run% info --verbose --no-color

%run% new -v --no-color

%run% info --verbose -nc

%run% new --non-existing-option

%run% new info

%run% new info --non-existing-option