# InsTrivia

InsTrivia is a full-stack trivia web application where players choose a category, answer seven randomized questions, receive a score, and compare their results on a category leaderboard. The application is designed to make the connection between a responsive React interface, an ASP.NET Core API, and persistent SQLite data easy to understand.

## Running the Application

### Requirements

- .NET 10 SDK
- Node.js and pnpm

### Start both servers

From the `app-react-frontend` directory, run:

```powershell
pnpm install
pnpm run dev:all
```

This starts both development servers:

- Frontend: http://localhost:5173
- Backend API: http://localhost:5069

Open http://localhost:5173 in a browser to use the application. The `dev:all` script uses `concurrently` to run Vite and ASP.NET Core together. Vite proxies `/api` requests to the backend.

## Application Flow

InsTrivia uses one React page with several interactive views:

1. **Username:** The player enters a username. The frontend sends it to the backend, which checks whether the name is available.
2. **Category:** The frontend loads categories from SQLite through the ASP.NET Core API. The player chooses one category.
3. **Quiz:** The API returns seven randomized questions for the selected category. Players choose an answer for each question.
4. **Results:** The backend grades the answers using the correct answers stored in SQLite. The frontend displays the score and reloads the category leaderboard.

The correct answers are never included in the public question response. Scores are calculated on the server to keep the result trustworthy.

## Software Demo Video

There is a Software Demo Video which can be given by request.

The video demonstrates the application running and explains the TypeScript, React, ASP.NET Core, SQLite, and overall project structure.

## Development Environment

The application was developed with:

- **Frontend:** React 19, TypeScript, Vite, and CSS
- **Backend:** ASP.NET Core Web API on .NET 10
- **Database:** SQLite
- **Data access:** Entity Framework Core and EF Core SQLite
- **Development tools:** Visual Studio Code, pnpm, and the .NET CLI
- **Supporting packages:** `concurrently` for starting both servers with one command

The frontend uses TypeScript types to describe categories, questions, scores, and leaderboard entries. React state controls the username, category, quiz, and results views. The ASP.NET Core controllers handle requests, validation, scoring, and leaderboard queries. Entity Framework Core maps the C# models to SQLite tables.

## Project Structure

```text
app-react-frontend/
	src/
		pages/App.tsx       Main React application and quiz flow
		App.css             Responsive visual design
		main.tsx            React entry point
	public/               Static assets and InsTrivia icon

app.asp.net.backend/
	Controllers/          API endpoints for categories, players, questions, and scores
	Data/TriviaData/      Database context, seed data, and initializer
	Dtos/TriviaDtos/       Public request and response shapes
	Models/TriviaModels/   Database entities
	Services/             Username availability helper
	Migrations/            Entity Framework Core database migrations
```

## Useful Websites

- [React Documentation](https://react.dev/)
- [TypeScript Documentation](https://www.typescriptlang.org/docs/)
- [ASP.NET Core Documentation](https://learn.microsoft.com/aspnet/core/)
- [Entity Framework Core Documentation](https://learn.microsoft.com/ef/core/)
- [SQLite Documentation](https://www.sqlite.org/docs.html)
- [Vite Documentation](https://vite.dev/guide/)

## Future Work

- Add a separate all-time leaderboard view across every category.
- Add stronger username rules and clearer duplicate-name feedback.
- Add automated API and frontend tests for the quiz and scoring flow.
- Add question-management tools so trivia content can be edited without changing the seed file.
- Add production deployment configuration for the React frontend and ASP.NET Core API.
