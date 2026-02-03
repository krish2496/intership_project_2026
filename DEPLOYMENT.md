# 🚀 Deployment Guide - Render (Backend) + Vercel (Frontend)

This guide will help you deploy your full-stack application with the backend on Render and frontend on Vercel.

---

## 📋 Prerequisites

Before you start, make sure you have:
- ✅ GitHub account with your code pushed to `https://github.com/krish2496/intership_project_2026.git`
- ✅ [Render account](https://render.com) (free tier available)
- ✅ [Vercel account](https://vercel.com) (free tier available)
- ✅ TMDB API key (optional, for movie features) - Get it from [TMDB](https://www.themoviedb.org/settings/api)

---

## 🔧 Part 1: Deploy Backend to Render

### Step 1: Create Render Account
1. Go to [https://render.com](https://render.com)
2. Click **"Get Started"** or **"Sign Up"**
3. Sign up with your GitHub account (recommended for easy integration)

### Step 2: Create PostgreSQL Database
1. From your Render dashboard, click **"New +"** → **"PostgreSQL"**
2. Fill in the details:
   - **Name**: `tracker-db`
   - **Database**: `trackerdb`
   - **User**: `trackeruser`
   - **Region**: Choose closest to you (e.g., Oregon)
   - **Plan**: **Free**
3. Click **"Create Database"**
4. ⏳ Wait for the database to be created (takes 1-2 minutes)
5. 📝 **IMPORTANT**: Copy the **Internal Database URL** (you'll need this later)
   - It looks like: `postgresql://trackeruser:password@dpg-xxx/trackerdb`

### Step 3: Deploy .NET Backend
1. From Render dashboard, click **"New +"** → **"Web Service"**
2. Click **"Connect a repository"** → Select your GitHub repository:
   - `krish2496/intership_project_2026`
3. Configure the web service:
   - **Name**: `tracker-api`
   - **Region**: Same as your database (e.g., Oregon)
   - **Branch**: `main`
   - **Runtime**: **Docker**
   - **Plan**: **Free**

4. **Environment Variables** - Click **"Add Environment Variable"** for each:
   
   | Key | Value |
   |-----|-------|
   | `ASPNETCORE_ENVIRONMENT` | `Production` |
   | `ASPNETCORE_URLS` | `http://+:8080` |
   | `ConnectionStrings__DefaultConnection` | Paste your **Internal Database URL** from Step 2 |
   | `Jwt__Issuer` | `TrackerApp` |
   | `Jwt__Audience` | `TrackerUsers` |
   | `Jwt__Key` | Generate a random 64+ character string (use [passwordsgenerator.net](https://passwordsgenerator.net/)) |
   | `Tmdb__ApiKey` | Your TMDB API key (or leave blank for now) |

5. Click **"Create Web Service"**
6. ⏳ Wait for deployment (takes 5-10 minutes for first deploy)
7. 📝 **IMPORTANT**: Once deployed, copy your backend URL:
   - It will be something like: `https://tracker-api.onrender.com`

### Step 4: Verify Backend is Running
1. Once deployment is complete, click on your service URL
2. Add `/swagger` to the URL: `https://tracker-api.onrender.com/swagger`
3. ✅ You should see the Swagger API documentation page

> [!NOTE]
> Free tier services on Render spin down after 15 minutes of inactivity. First request after inactivity may take 30-60 seconds.

---

## 🎨 Part 2: Deploy Frontend to Vercel

### Step 1: Create Vercel Account
1. Go to [https://vercel.com](https://vercel.com)
2. Click **"Sign Up"**
3. Sign up with your GitHub account

### Step 2: Import Your Project
1. From Vercel dashboard, click **"Add New..."** → **"Project"**
2. Import your GitHub repository:
   - Select `krish2496/intership_project_2026`
3. Click **"Import"**

### Step 3: Configure Project Settings
1. **Framework Preset**: Select **"Next.js"**
2. **Root Directory**: Click **"Edit"** and change to `./client`
3. **Build and Output Settings**:
   - Build Command: `npm run build` (auto-detected)
   - Output Directory: `.next` (auto-detected)
   - Install Command: `npm install` (auto-detected)

### Step 4: Add Environment Variables
Click **"Environment Variables"** and add:

| Name | Value |
|------|-------|
| `NEXT_PUBLIC_API_URL` | Your Render backend URL (e.g., `https://tracker-api.onrender.com`) |
| `NEXT_PUBLIC_TMDB_API_KEY` | Your TMDB API key (optional) |

### Step 5: Deploy
1. Click **"Deploy"**
2. ⏳ Wait for deployment (takes 2-5 minutes)
3. 🎉 Once complete, Vercel will give you a URL like: `https://intership-project-2026.vercel.app`

### Step 6: Test Your Application
1. Visit your Vercel URL
2. Try signing up / logging in
3. Test creating polls and voting

---

## 🔄 Updating Your Deployment

### Update Backend (Render)
Render automatically deploys when you push to GitHub:
```bash
git add .
git commit -m "Update backend"
git push origin main
```
Render will detect the push and redeploy automatically.

### Update Frontend (Vercel)
Vercel also auto-deploys on push:
```bash
git add .
git commit -m "Update frontend"
git push origin main
```
Vercel will detect the push and redeploy automatically.

---

## 🐛 Troubleshooting

### Backend Issues

**Problem**: Database connection error
- **Solution**: Verify the `ConnectionStrings__DefaultConnection` environment variable is set correctly with the Internal Database URL

**Problem**: 502 Bad Gateway
- **Solution**: Check Render logs. Go to your service → "Logs" tab to see what's wrong

**Problem**: CORS errors
- **Solution**: The backend is configured to allow all origins. If you still see CORS errors, check that your frontend is using the correct API URL

### Frontend Issues

**Problem**: API calls failing
- **Solution**: Verify `NEXT_PUBLIC_API_URL` is set correctly in Vercel environment variables

**Problem**: Build fails
- **Solution**: Check Vercel build logs. Common issues:
  - Missing dependencies
  - TypeScript errors
  - Environment variables not set

**Problem**: 404 on routes
- **Solution**: Next.js should handle this automatically. Check that your `vercel.json` is committed to git

---

## 📊 Monitoring

### Render Dashboard
- View logs: Service → "Logs" tab
- View metrics: Service → "Metrics" tab
- Database stats: Database → "Info" tab

### Vercel Dashboard
- View deployments: Project → "Deployments" tab
- View analytics: Project → "Analytics" tab
- View logs: Deployment → "Logs" tab

---

## 💰 Cost

Both services are **completely free** for this project:
- **Render Free Tier**: 750 hours/month (enough for one service running 24/7)
- **Vercel Free Tier**: Unlimited personal projects

---

## 🎯 Next Steps

1. ✅ Deploy backend to Render
2. ✅ Deploy frontend to Vercel
3. ✅ Test your application
4. 🚀 Share your live URL with others!

---

## 📝 Important URLs

After deployment, save these URLs:

- **Frontend**: `https://intership-project-2026.vercel.app` (your actual URL will be different)
- **Backend API**: `https://tracker-api.onrender.com` (your actual URL will be different)
- **Backend Swagger**: `https://tracker-api.onrender.com/swagger`

---

## ❓ Need Help?

If you run into issues:
1. Check the troubleshooting section above
2. Review Render/Vercel logs
3. Verify all environment variables are set correctly
4. Make sure your GitHub repository is up to date
