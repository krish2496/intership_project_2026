# 📺 Anime & TV Series Tracker — Complete Project Documentation

> **Project Name**: Internship Project 2026 (Tracker App)  
> **Repository**: [github.com/krish2496/intership_project_2026](https://github.com/krish2496/intership_project_2026)  
> **Last Updated**: April 2026

---

## 📋 Table of Contents

1. [Project Overview](#1-project-overview)
2. [Technology Stack](#2-technology-stack)
3. [System Architecture](#3-system-architecture)
4. [Backend — Server Layer Details](#4-backend--server-layer-details)
5. [Database Schema & Entities](#5-database-schema--entities)
6. [API Endpoint Catalog](#6-api-endpoint-catalog)
7. [Frontend — Client Layer Details](#7-frontend--client-layer-details)
8. [External API Integrations](#8-external-api-integrations)
9. [Authentication & Security](#9-authentication--security)
10. [Project Features & Modules](#10-project-features--modules)
11. [Development Workflow](#11-development-workflow)
12. [Deployment Workflow](#12-deployment-workflow)
13. [Environment Variables Reference](#13-environment-variables-reference)
14. [Data Flow Diagrams](#14-data-flow-diagrams)

---

## 1. Project Overview

The **Anime & TV Series Tracker** is a full-stack community web application that enables users to:

- **Track** their anime and TV series watch progress with detailed statuses
- **Discover** new anime via the Jikan API (MyAnimeList) and TV shows via TMDB
- **Engage socially** through clubs, discussion boards, polls, and comments
- **Follow friends** and receive activity feeds
- **Compete** on leaderboards based on watch activity
- **Receive recommendations** based on their watch history

The application is built on a **Clean Architecture** pattern (.NET 8 backend), a modern **React/Next.js** frontend, and a **PostgreSQL** relational database.

---

## 2. Technology Stack

### Frontend

| Technology | Version | Purpose |
|------------|---------|---------|
| **Next.js** | 16.1.4 | React framework with App Router, SSR/SSG |
| **React** | 19.2.3 | UI component library |
| **TypeScript** | ^5 | Static type-checking for JavaScript |
| **TailwindCSS** | ^4 | Utility-first CSS framework |
| **Axios** | ^1.13.2 | HTTP client for API calls |
| **js-cookie** | ^3.0.5 | Cookie-based JWT token management |
| **react-hook-form** | ^7.71.1 | Form state management & validation |
| **react-toastify** | ^11.0.5 | Toast notification system |
| **date-fns** | ^4.1.0 | Date utility functions |
| **ESLint** | ^9 | Code linting & quality enforcement |

### Backend

| Technology | Version | Purpose |
|------------|---------|---------|
| **ASP.NET Core** | .NET 8 | Web API framework |
| **Entity Framework Core** | Latest | ORM & database migrations |
| **Npgsql** | Latest | PostgreSQL EF Core provider |
| **JWT Bearer** | Latest | JSON Web Token authentication |
| **Swagger / Swashbuckle** | Latest | API documentation & testing UI |
| **C#** | 12 | Primary backend language |

### Database

| Technology | Purpose |
|------------|---------|
| **PostgreSQL** | Primary relational database |
| **Entity Framework Migrations** | Schema version control |

### Infrastructure & DevOps

| Technology | Purpose |
|------------|---------|
| **Docker** | Containerized backend deployment |
| **Render** | Cloud hosting for .NET backend API + PostgreSQL DB |
| **Vercel** | Cloud hosting for Next.js frontend |
| **GitHub** | Source control, CI/CD trigger |

### External APIs

| API | Purpose |
|-----|---------|
| **Jikan API v4** | Anime data from MyAnimeList (free, no key needed) |
| **TMDB API** | TV Series and movie metadata |

---

## 3. System Architecture

### High-Level Architecture

```
+------------------------------------------------------------------+
|                        CLIENT (Browser)                          |
|  Next.js 16 + React 19 + TypeScript + TailwindCSS               |
|  Hosted on: Vercel (intership-project-2026.vercel.app)          |
+-------------------------+----------------------------------------+
                          |  HTTPS REST API calls
                          |  JWT in Authorization header
                          v
+------------------------------------------------------------------+
|                   BACKEND API (.NET 8)                           |
|  ASP.NET Core Web API                                            |
|  Hosted on: Render (intership-project-2026.onrender.com)        |
|                                                                   |
|  Controllers (14)  -->  Services (14)  -->  Infrastructure       |
|                                               (EF Core, Auth)    |
|                                                                   |
|   [Jikan API]                         [TMDB API]                 |
|   Anime data                          TV Series data             |
+------------------------------------------------------------------+
                          |  EF Core + Npgsql
                          v
+------------------------------------------------------------------+
|                     PostgreSQL Database                           |
|  Hosted on: Render Managed PostgreSQL                            |
|  ~20 tables covering all domain entities                         |
+------------------------------------------------------------------+
```

### Backend Clean Architecture Layers

```
server/
├── Tracker.API            <- Presentation Layer (Controllers, Middleware)
├── Tracker.Services       <- Application Layer (Business Logic)
├── Tracker.Core           <- Domain Layer (Entities, Interfaces, DTOs, Enums)
└── Tracker.Infrastructure <- Infrastructure Layer (EF Core, Auth, External APIs)
```

**Dependency Rule**: Each layer only depends inward.
`Tracker.API` -> `Tracker.Services` -> `Tracker.Core` <- `Tracker.Infrastructure`

---

## 4. Backend — Server Layer Details

### 4.1 Tracker.API (Presentation Layer)

**Purpose**: Exposes HTTP REST endpoints, handles request routing, CORS, authentication pipeline.

| File | Description |
|------|-------------|
| `Program.cs` | App bootstrap, middleware pipeline, DI registration |
| `appsettings.json` | Connection strings, JWT settings, TMDB key |
| `Controllers/*.cs` | 14 controllers mapping HTTP routes to services |
| `Data/DbSeeder.cs` | Seeds sample anime data on startup |

**Middleware Pipeline Order:**
1. Swagger / SwaggerUI
2. CORS (AllowFrontend policy)
3. Static Files
4. Authentication (JWT Bearer)
5. Authorization
6. Controller mapping
7. Fallback to index.html

**CORS Allowed Origins:**
- https://intership-project-2026-la9p.vercel.app
- https://intership-project-2026.vercel.app
- http://localhost:3000
- http://localhost:5173

---

### 4.2 Tracker.Services (Application Layer)

| Service | Responsibility |
|---------|---------------|
| `AuthService` | Register/login, password hashing |
| `WatchlistService` | Add/update/remove watchlist entries |
| `MediaService` | Manage local media records |
| `ClubService` | Create/join/leave clubs |
| `DiscussionService` | Forum discussions per club |
| `CommentService` | Nested comments with like/dislike |
| `PollService` | Create polls, cast votes |
| `ProfileService` | User profile and statistics |
| `SocialService` | Follow/unfollow, followers/following |
| `ActivityService` | User activity feed tracking |
| `AdminService` | Admin-only user management |
| `RecommendationService` | Media recommendations from watch history |

---

### 4.3 Tracker.Core (Domain Layer)

Sub-folders:
- **Entities/** — 14 domain entity classes mapped to DB tables
- **Interfaces/** — 16 service and repository interfaces
- **DTOs/** — Data Transfer Objects for request/response shaping
- **Enums/** — Domain enumerations

```csharp
enum MediaType   { Anime = 0, TVSeries = 1 }
enum WatchStatus { Watching = 0, Completed = 1, OnHold = 2, Dropped = 3, PlanToWatch = 4 }
```

---

### 4.4 Tracker.Infrastructure (Infrastructure Layer)

| File/Folder | Description |
|------------|-------------|
| `Data/TrackerDbContext.cs` | EF Core DbContext with all configurations |
| `Data/Repositories/` | UserRepository implementation |
| `Auth/TokenService.cs` | JWT token generation (HS512) |
| `External/Jikan/JikanService.cs` | HttpClient for Jikan Anime API |
| `External/TMDB/TmdbService.cs` | HttpClient for TMDB TV Series API |
| `DependencyInjection.cs` | Extension to register all infrastructure services |

---

## 5. Database Schema & Entities

### Entity Relationship Overview

```
Users
  |--- Watchlist ------> Media
  |--- OwnedClubs ------> Clubs
  |                         |--- Discussions -> Comments -> CommentLikes
  |                         |--- ClubEvents -> ClubEventAttendees
  |                         |--- ClubLists -> ClubListItems
  |                         |--- ClubInvites
  |--- Polls -> PollOptions -> PollVotes
  |--- Follows (self-referential)
  |--- Activities
```

### Entity Definitions

| Entity | Key Fields | Description |
|--------|-----------|-------------|
| **User** | Id, Username, Email, PasswordHash, Role, ThemePreference, CreatedAt | Core user account |
| **Media** | Id, Title, ExternalId, Type, TotalEpisodes, Description, CoverImageUrl | Anime or TV series |
| **Watchlist** | Id, UserId, MediaId, Status, CurrentEpisode, Rating, Notes | Watch tracking entry |
| **Club** | Id, Name, Description, OwnerId, IsPrivate, ActivityScore, Level | Community club |
| **Discussion** | Id, ClubId, UserId, Title, Content, IsSpoiler, CreatedAt | Club forum topic |
| **Comment** | Id, DiscussionId, UserId, Content, ParentCommentId | Nested comment/reply |
| **CommentLike** | Id, CommentId, UserId, IsLike | Like or dislike |
| **Poll** | Id, CreatorId, Question, ClubId, ExpiresAt | Community poll |
| **PollOption** | Id, PollId, Text | Poll choice |
| **PollVote** | PollId, UserId, PollOptionId | Vote (1 per poll per user) |
| **ClubEvent** | Id, ClubId, CreatorId, Title, EventDate, Location | Club event |
| **ClubEventAttendee** | EventId, UserId, RSVP | Event attendance |
| **ClubList** | Id, ClubId, CreatorId, Name | Shared media list |
| **ClubListItem** | Id, ListId, MediaId, AddedById | Item in club list |
| **ClubInvite** | Id, ClubId, InviterId, InviteeId, Status | Club invitation |
| **Follow** | FollowerId, FollowingId (composite PK) | Follow relationship |
| **Activity** | Id, UserId, Type, Description, TargetId, CreatedAt | Activity feed entry |

### Key Database Constraints

| Constraint | Detail |
|-----------|--------|
| Users.Username | Unique index |
| Users.Email | Unique index |
| PollVote(PollId, UserId) | Unique — one vote per poll |
| ClubEventAttendee(EventId, UserId) | Unique — one RSVP per event |
| ClubInvite(ClubId, InviteeId) | Unique — one invite per club |
| Follow(FollowerId, FollowingId) | Composite primary key |

---

## 6. API Endpoint Catalog

> **Base URL**: `https://intership-project-2026.onrender.com/api`
> **Swagger UI**: `https://intership-project-2026.onrender.com/swagger`

### Authentication

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | /auth/register | No | Register new user |
| POST | /auth/login | No | Login, get JWT token |

### Watchlist

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | /watchlist | Yes | Get user's watchlist |
| POST | /watchlist | Yes | Add to watchlist |
| PUT | /watchlist/{id} | Yes | Update status/episode/rating |
| DELETE | /watchlist/{id} | Yes | Remove from watchlist |

### Media

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | /media/anime/search?query= | No | Search anime via Jikan |
| GET | /media/tvseries/search?query= | No | Search TV via TMDB |
| GET | /media/anime/{malId} | No | Anime detail by MAL ID |

### Clubs

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | /clubs | No | List all clubs |
| POST | /clubs | Yes | Create club |
| GET | /clubs/{id} | No | Club details |
| POST | /clubs/{id}/join | Yes | Join club |
| DELETE | /clubs/{id}/leave | Yes | Leave club |

### Discussions

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | /discussions/{clubId} | No | List discussions |
| POST | /discussions | Yes | Create discussion |
| DELETE | /discussions/{id} | Yes | Delete discussion |

### Comments

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | /comments/{discussionId} | No | Get comments |
| POST | /comments | Yes | Post comment |
| DELETE | /comments/{id} | Yes | Delete comment |
| POST | /comments/{id}/like | Yes | Like comment |
| POST | /comments/{id}/dislike | Yes | Dislike comment |

### Polls

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | /polls | No | List polls |
| POST | /polls | Yes | Create poll |
| GET | /polls/{id} | No | Poll details + votes |
| POST | /polls/{id}/vote | Yes | Cast vote |

### Social

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | /social/follow/{userId} | Yes | Follow user |
| DELETE | /social/unfollow/{userId} | Yes | Unfollow user |
| GET | /social/followers/{userId} | No | User's followers |
| GET | /social/following/{userId} | No | User's following |

### Profile

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | /profile | Yes | Own profile + stats |
| GET | /profile/{username} | No | Public profile |
| PUT | /profile | Yes | Update profile |

### Leaderboard, Feed, Stats

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | /leaderboard | No | Ranked users by watches |
| GET | /feed | Yes | Activity from followed users |
| GET | /stats/public | No | App-wide statistics |

### Admin (Admin Role Required)

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | /admin/users | Admin | List all users |
| DELETE | /admin/users/{id} | Admin | Delete user |

---

## 7. Frontend — Client Layer Details

### Directory Structure

```
client/src/
├── app/
│   ├── page.tsx              <- Home/Landing
│   ├── layout.tsx            <- Root layout
│   ├── globals.css           <- Global styles
│   ├── auth/                 <- Login & Registration
│   ├── anime/                <- Anime search & detail
│   ├── dashboard/            <- Watchlist dashboard
│   ├── profile/              <- User profile
│   ├── clubs/                <- Clubs & discussions
│   ├── search/               <- Global search
│   ├── feed/                 <- Activity feed
│   ├── leaderboard/          <- Rankings
│   ├── users/                <- Browse users
│   └── admin/                <- Admin panel
├── components/
│   ├── Navbar.tsx            <- Navigation bar
│   ├── PollCard.tsx          <- Poll voting card
│   ├── Providers.tsx         <- Context providers
│   └── ui/                   <- UI primitives
├── context/
│   └── AuthContext           <- Auth state management
├── lib/
│   └── api.ts                <- Axios client + JWT interceptor
└── services/
    └── socialService.ts      <- Social API calls
```

### Page Routes

| Route | Page/Feature |
|-------|-------------|
| / | Landing page with public stats |
| /auth/login | JWT-based login |
| /auth/register | New account registration |
| /dashboard | Personal watchlist management |
| /anime | Anime browsing & search |
| /anime/[id] | Anime detail + add to watchlist |
| /profile | Own user profile |
| /profile/[username] | Any user's public profile |
| /clubs | All clubs listing |
| /clubs/[id] | Club detail with discussions |
| /feed | Social activity feed |
| /leaderboard | Global user rankings |
| /users | Browse/follow users |
| /search | Cross-media search |
| /admin | Admin management panel |

---

## 8. External API Integrations

### Jikan API (Anime)
- **URL**: https://api.jikan.moe/v4
- **Auth**: None required
- **Use Cases**: Search anime, fetch by MAL ID
- **Implementation**: JikanService.cs (IAnimeService)

### TMDB API (TV Series)
- **URL**: https://api.themoviedb.org/3
- **Auth**: API key in configuration
- **Use Cases**: Search TV series, fetch details
- **Implementation**: TmdbService.cs (ITvSeriesService)

---

## 9. Authentication & Security

### JWT Configuration

| Property | Value |
|---------|-------|
| Algorithm | HMAC-SHA512 |
| Issuer | TrackerApp |
| Audience | TrackerUsers |
| Storage | Browser Cookie (token) |
| Header | Authorization: Bearer <token> |

### Roles

| Role | Access |
|------|--------|
| User | All standard features |
| Admin | Admin endpoints + all user features |

### Security Measures
- Passwords hashed before storage
- JWT auto-attached via Axios interceptor
- CORS strict allow-list (no wildcard)
- Database-level uniqueness constraints prevent duplicate votes/RSVPs

---

## 10. Project Features & Modules

### Media Tracking
- Search anime (Jikan/MAL) and TV series (TMDB)
- Watch statuses: Watching, Completed, On Hold, Dropped, Plan to Watch
- Track current episode, 1-10 rating, personal notes

### Clubs & Communities
- Public/private clubs with ownership
- Discussion boards with nested comments
- Like/dislike on comments
- Club events with RSVP
- Collaborative media lists
- Club invitations

### Polls
- Multi-option polls (with optional club association)
- One vote per user enforced at DB level
- Poll expiry dates

### Social Features
- Follow/unfollow users
- Activity feed from followed accounts
- Leaderboard by completed titles
- Public user profiles

### Recommendations
- Algorithm suggests media based on watch history genres

### Admin Panel
- View and delete user accounts (Admin role only)

---

## 11. Development Workflow

### Setup Requirements

| Tool | Version |
|------|---------|
| Node.js | >= 18 |
| .NET SDK | 8.0 |
| PostgreSQL | 14+ |
| Git | Latest |

### Local Development Steps

```bash
# 1. Clone
git clone https://github.com/krish2496/intership_project_2026.git
cd "intership_project_2026"

# 2. Apply DB migrations
cd server
dotnet ef database update --project Tracker.Infrastructure --startup-project Tracker.API

# 3. Start backend
cd Tracker.API
dotnet run
# API: http://localhost:5000 | Swagger: http://localhost:5000/swagger

# 4. Start frontend (new terminal)
cd ../../client
npm install
npm run dev
# App: http://localhost:3000
```

### Git Workflow

```
main <- production (auto-deploys Render + Vercel)
  |--- feature/[name] <- development work
```

---

## 12. Deployment Workflow

### CI/CD: Push to `main` triggers:
- **Render**: Docker build -> dotnet publish -> container deployed
- **Vercel**: Next.js build -> optimized output -> CDN deployed

### Dockerfile (Backend)

```dockerfile
# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
COPY server ./server
RUN dotnet publish Tracker.API.csproj -c Release -o /app

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Tracker.API.dll"]
```

### Production URLs

| Service | URL |
|---------|-----|
| Frontend | https://intership-project-2026.vercel.app |
| Backend API | https://intership-project-2026.onrender.com |
| Swagger UI | https://intership-project-2026.onrender.com/swagger |

---

## 13. Environment Variables Reference

### Backend (Render)

| Variable | Value | Required |
|----------|-------|----------|
| ASPNETCORE_ENVIRONMENT | Production | Yes |
| ASPNETCORE_URLS | http://+:8080 | Yes |
| ConnectionStrings__DefaultConnection | PostgreSQL connection string | Yes |
| Jwt__Key | Secret key (64+ chars) | Yes |
| Jwt__Issuer | TrackerApp | Yes |
| Jwt__Audience | TrackerUsers | Yes |
| Tmdb__ApiKey | TMDB API key | Optional |

### Frontend (Vercel)

| Variable | Value | Required |
|----------|-------|----------|
| NEXT_PUBLIC_API_URL | Backend API URL | Yes |
| NEXT_PUBLIC_TMDB_API_KEY | TMDB key | Optional |

---

## 14. Data Flow Diagrams

### Authentication Flow

```
Browser -> POST /auth/login { username, password }
        <- Backend verifies password hash
        <- Returns JWT token
Browser stores token in cookie
All future requests include: Authorization: Bearer <token>
```

### Media Search & Watchlist Flow

```
Browser -> GET /media/anime?q=Naruto
Backend -> Calls Jikan API
        <- Returns anime list
Browser <- Displays results

Browser -> POST /watchlist { mediaId, status }
Backend -> Find or create Media record in DB
        -> Create Watchlist entry in DB
        <- 201 Created
```

### Club Discussion Flow

```
User A -> POST /discussions { clubId, title, content }
       <- 201 { discussion }

User A -> POST /comments { discussionId, content }
       <- 201 { comment }

User B -> GET /comments/{discussionId}
       <- comment list

User B -> POST /comments/{id}/like
       <- 200 OK
```

---

## Project Summary Metrics

| Category | Count |
|----------|-------|
| Backend Controllers | 14 |
| Backend Services | 14 |
| Core Interfaces | 16 |
| Domain Entities | 17 |
| Database Tables | ~20 |
| Frontend Pages | 14+ |
| External APIs | 2 (Jikan, TMDB) |
| Deployment Platforms | 3 (Render API, Render DB, Vercel) |

---

*Document generated: April 2026 | Repository: krish2496/intership_project_2026*
