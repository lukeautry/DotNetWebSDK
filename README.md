# Overview

Keeping your .NET classes in sync with your client-side (web-based) code is annoying and a breeding ground for bugs. What if you could have a single source of truth in your C# code?

The concept is already well known with ORMs, notably Entity Framework. Your build your POCOs, register them properly, and the database schema with the correct relationships is automatically built out by EF. You've probably heard the term "code first migrations" in EF.

The goal of this project is to implement something similar to code first migrations, except instead of .NET models => Database Schema, it's .NET models => TypeScript model classes and an API access layer for AJAX calls.

# Usage

Coming soon. Check out http://lukeautry.com/Blog/BuildingDotNetWebSDK to read more about the project.