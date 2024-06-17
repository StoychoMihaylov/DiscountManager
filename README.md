# DiscountManager Project

## Overview

This project consists of a gRPC server and a console client application to manage discount codes. The server is responsible for generating and storing discount codes in MongoDB, while the client interacts with the server via gRPC to generate and retrieve discount codes.

## Setup

### Prerequisites

- Docker
- Docker Compose

### Build and Run with Docker Compose

1. Clone the repository:

    ```bash
    git clone https://github.com/your-repo/discountmanager.git
    cd discountmanager
    ```

2. Build the Docker images:

    ```bash
    docker-compose build
    ```

3. Start the services:

    ```bash
    docker-compose up
    ```

4. Open another terminal and attach to the client container:

    ```bash
    docker attach discountmanager-client
    ```

## Usage

### Commands

Once attached to the client container, you can use the following commands:

- **Generate Discount Codes:**

    ```bash
    run create-discount-codes
    ```

    Follow the prompt to enter the number of discount codes to generate (maximum 2000).

- **Retrieve All Discount Codes:**

    ```bash
    run get-all-discount-codes
    ```

## Project Structure

- **discountmanager-server:** The gRPC server that handles discount code generation and storage.
- **discountmanager-client:** The console client application that interacts with the server via gRPC.
