# MudXComponents

## Extra UI Components for Mudblazor 
On top of mudblazor, MudXComponents is a component library that offers more capabilities. Although Mudblazor is great, I find that using some of its features, such the tables and select boxes, takes a lot of time and effort. As a result, so much effort is spent on the frontend. I thus made the decision to develop certain components that would be significantly more time- and resource-efficient to use.


Setup:

1. Install via nuget


 <div>
   <span>dotnet add package MudXComponents --version 1.0.2</span>   
 </div>
        
        
2. Register in program.cs


List of offered components (More will be coming soon)
MudGridX
MudSelectX 

## MudGridX

I wanted MudGridX to be as simple to use as possible while also delivering the finest capabilities possible,such as auto-generated UI for Create, Update, and Delete actions.

 
MudGridX has the following fragments:
1. GridColumns, 
2. GridButtons,
3. HeaderButtons

Required parameters for MudGridX
 
1. TModel 
2. DataSource 
3. EnableSearch  
4. OnCreate
5. OnDelete 
6. OnUpdate


Sample Usage in examples project. More will be added soon

 


