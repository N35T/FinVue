FROM node:22.16.0 as build
WORKDIR /app

RUN npm install -g @angular/cli

COPY ./package*.json ./
RUN npm ci
COPY . .

ENV API_BASE_URL=/api

RUN npm run config

RUN ng build --configuration=production

FROM nginx:1.21-alpine
COPY --from=build /app/dist/FinVue.UI/browser /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]