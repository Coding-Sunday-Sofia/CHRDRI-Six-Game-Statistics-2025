<?php
header("Content-Type: application/json");

$dsn = "pgsql:host=db;port=5432;dbname=mydb;";
$user = "myuser";
$pass = "mypassword";

try {
    $pdo = new PDO($dsn, $user, $pass);
} catch (PDOException $e) {
    echo "Error: " . $e->getMessage();
}

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $input = json_decode(file_get_contents("php://input"), true);

    if (!isset($input['gguid']) || !isset($input['turn']) || !isset($input['board'])) {
        http_response_code(400);
        echo json_encode(["error" => "Missing required fields"]);
        exit;
    }

    try {
        $stmt = $pdo->prepare("INSERT INTO statistics (gguid, turn, board) VALUES (:gguid, :turn, :board)");
        $stmt->execute([
            ':gguid' => $input['gguid'],
            ':turn' => $input['turn'],
            ':board' => $input['board']
        ]);

        http_response_code(201);
        echo json_encode(["success" => true, "id" => $pdo->lastInsertId()]);
    } catch (PDOException $e) {
        http_response_code(500);
        echo json_encode(["error" => $e->getMessage()]);
    }
} else {
    http_response_code(405);
    echo json_encode(["error" => "Method not allowed"]);
}
?>

