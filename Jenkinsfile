pipeline {
    agent any

    environment {
        APP_NAME       = 'earn-dotnet-api'                       // change to your app's name
        IMAGE_NAME     = '192.168.238.50:5000/earn-dotnet-api'   // <registry>/<app_name>
        CONTAINER_NAME = 'earn-dotnet-api'                        // must match modules/nginx's upstream_container if fronted by Nginx
        APP_PORT       = '8070'                                   // port the API listens on inside the container
        DOCKER_NETWORK = 'monitoring'                             // shared network so Nginx can resolve this container by name
    }

    options {
        timestamps()
        buildDiscarder(logRotator(numToKeepStr: '10'))
    }

    stages {

        stage('Restore') {
            steps {
                sh 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                sh 'dotnet build --configuration Release --no-restore'
            }
        }

        stage('Test') {
            steps {
                sh 'dotnet test --configuration Release --no-build --logger "trx;LogFileName=test-results.trx"'
            }
            post {
                always {
                    junit testResults: '**/test-results.trx', allowEmptyResults: true
                }
            }
        }

        stage('Publish') {
            steps {
                sh 'dotnet publish --configuration Release --no-build -o ./publish'
            }
        }

        stage('Docker Build') {
            steps {
                sh """
                    docker build \
                      -t $IMAGE_NAME:$BUILD_NUMBER \
                      -t $IMAGE_NAME:latest \
                      .
                """
            }
        }

        stage('Docker Push') {
            steps {
                sh """
                    docker push $IMAGE_NAME:$BUILD_NUMBER
                    docker push $IMAGE_NAME:latest
                """
            }
        }

        stage('Deploy') {
            steps {
                sh """
                    docker rm -f $CONTAINER_NAME || true

                    docker run -d \
                      --name $CONTAINER_NAME \
                      --network $DOCKER_NETWORK \
                      --restart unless-stopped \
                      -e ASPNETCORE_URLS=http://+:$APP_PORT \
                      $IMAGE_NAME:$BUILD_NUMBER
                """
            }
        }
    }

    post {
        success {
            echo "Pipeline succeeded: ${APP_NAME} build #${BUILD_NUMBER} deployed."
        }
        failure {
            echo "Pipeline failed for ${APP_NAME} build #${BUILD_NUMBER}."
        }
        always {
            cleanWs()
        }
    }
}
