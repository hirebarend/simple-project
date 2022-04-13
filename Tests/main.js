const axios = require("axios").default;
const uuid = require("uuid");

(async () => {
  const timestamp1 = new Date().getTime();

  //   for (let i = 0; i < 50; i++) { // 166
  //     await execute();
  //   }

  const array = [];

  for (let i = 0; i < 1; i++) {
    // 18
    array.push(execute());
  }

  await Promise.all(array);

  const timestamp2 = new Date().getTime();

  console.log(`${(timestamp2 - timestamp1) / 1000} seconds`);
})();

async function execute() {
  try {
    const reference = uuid.v4();

    const responsePost = await axios.post(
      `http://localhost:7071/api/Order/${reference}`
    );

    while (true) {
      const responseGet = await axios.get(
        `http://localhost:7071/api/Order/${reference}`
      );

      if (
        (responseGet.data && responseGet.data.state === 3) ||
        responseGet.data.state === 4 ||
        responseGet.data.state === 5 ||
        responseGet.data.state === 6
      ) {
        console.log(
          `${
            new Date(responseGet.data.updated).getTime() -
            new Date(responseGet.data.created).getTime()
          }`
        );

        break;
      }

      await new Promise((resolve) => setTimeout(() => resolve(), 100));
    }
  } catch {
    console.log(`[ERROR]`);
  }
}
