pipeline {
    agent any

    environment {
        APP_NAME       = 'earn-dotnet-api'
        IMAGE_NAME     = '192.168.238.50:5000/earn-dotnet-api'
        CONTAINER_NAME = 'earn-dotnet-api'
        APP_PORT       = '8070'
        DOCKER_NETWORK = 'monitoring'
    }

    options {
        timestamps()
        buildDiscarder(logRotator(numToKeepStr: '10'))
    }

    stages {

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