# Use an official Nginx image as the base image
FROM node:16-alpine AS build
WORKDIR /app

COPY src ./src
COPY *.json .
RUN npm install
RUN npm run build-prod
# Serve Application using Nginx Server
FROM nginx:alpine
COPY --from=build /app/dist/smart-expense-management-web/ /usr/share/nginx/html
EXPOSE 80
