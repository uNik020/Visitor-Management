#!/bin/bash

# Navigate to app folder
cd src/app || exit

# Create main folders
mkdir components pages services guards interceptors utils models

# Create common reusable component folders
mkdir -p components/navbar
mkdir -p components/footer

# Create main pages
mkdir -p pages/login
mkdir -p pages/register
mkdir -p pages/dashboard
mkdir -p pages/visitors
mkdir -p pages/appointments

# Sample service files (empty)
touch services/auth.service.ts
touch services/visitor.service.ts

# Sample guard
touch guards/auth.guard.ts

# Sample interceptor
touch interceptors/auth.interceptor.ts

# Sample util
touch utils/format-date.util.ts

# Sample model
touch models/visitor.model.ts

echo "✅ Clean Angular project structure created successfully!"
