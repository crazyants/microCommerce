FROM nginx

COPY ./app-server.conf /etc/nginx/conf.d/app-server.conf
COPY ./nginx.conf /etc/nginx/nginx.conf
RUN [ "mv", "/etc/nginx/conf.d/default.conf", "/etc/nginx/conf.d/default.conf_" ]