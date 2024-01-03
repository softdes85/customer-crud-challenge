# Stage 1: Build the application
FROM node:latest as build
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm install
COPY . .
RUN npm run build
# Assuming the dist folder is created in the parent directory of /app

# Stage 2: Serve the application
FROM nginx:alpine
COPY --from=build /app/dist/customer-management/browser /usr/share/nginx/html
# Copying from the /dist folder at the root level of the container
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]