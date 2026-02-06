namespace Demo
{
    internal class BabelOutput
    {
        public const string AstJson = """
            {
              "type": "File",
              "start": 0,
              "end": 109,
              "loc": {
                "start": {
                  "line": 1,
                  "column": 0,
                  "index": 0
                },
                "end": {
                  "line": 7,
                  "column": 9,
                  "index": 109
                },
                "filename": null,
                "identifierName": null
              },
              "errors": [],
              "program": {
                "type": "Program",
                "start": 0,
                "end": 109,
                "loc": {
                  "start": {
                    "line": 1,
                    "column": 0,
                    "index": 0
                  },
                  "end": {
                    "line": 7,
                    "column": 9,
                    "index": 109
                  },
                  "filename": null,
                  "identifierName": null
                },
                "sourceType": "script",
                "interpreter": null,
                "body": [
                  {
                    "type": "FunctionDeclaration",
                    "start": 0,
                    "end": 58,
                    "loc": {
                      "start": {
                        "line": 1,
                        "column": 0,
                        "index": 0
                      },
                      "end": {
                        "line": 4,
                        "column": 1,
                        "index": 58
                      },
                      "filename": null,
                      "identifierName": null
                    },
                    "id": {
                      "type": "Identifier",
                      "start": 9,
                      "end": 13,
                      "loc": {
                        "start": {
                          "line": 1,
                          "column": 9,
                          "index": 9
                        },
                        "end": {
                          "line": 1,
                          "column": 13,
                          "index": 13
                        },
                        "filename": null,
                        "identifierName": "add2"
                      },
                      "name": "add2"
                    },
                    "generator": false,
                    "async": false,
                    "params": [
                      {
                        "type": "Identifier",
                        "start": 14,
                        "end": 22,
                        "loc": {
                          "start": {
                            "line": 1,
                            "column": 14,
                            "index": 14
                          },
                          "end": {
                            "line": 1,
                            "column": 22,
                            "index": 22
                          },
                          "filename": null,
                          "identifierName": "a"
                        },
                        "name": "a",
                        "typeAnnotation": {
                          "type": "TSTypeAnnotation",
                          "start": 15,
                          "end": 22,
                          "loc": {
                            "start": {
                              "line": 1,
                              "column": 15,
                              "index": 15
                            },
                            "end": {
                              "line": 1,
                              "column": 22,
                              "index": 22
                            },
                            "filename": null,
                            "identifierName": null
                          },
                          "typeAnnotation": {
                            "type": "TSNumberKeyword",
                            "start": 16,
                            "end": 22,
                            "loc": {
                              "start": {
                                "line": 1,
                                "column": 16,
                                "index": 16
                              },
                              "end": {
                                "line": 1,
                                "column": 22,
                                "index": 22
                              },
                              "filename": null,
                              "identifierName": null
                            }
                          }
                        }
                      },
                      {
                        "type": "Identifier",
                        "start": 24,
                        "end": 32,
                        "loc": {
                          "start": {
                            "line": 1,
                            "column": 24,
                            "index": 24
                          },
                          "end": {
                            "line": 1,
                            "column": 32,
                            "index": 32
                          },
                          "filename": null,
                          "identifierName": "b"
                        },
                        "name": "b",
                        "typeAnnotation": {
                          "type": "TSTypeAnnotation",
                          "start": 25,
                          "end": 32,
                          "loc": {
                            "start": {
                              "line": 1,
                              "column": 25,
                              "index": 25
                            },
                            "end": {
                              "line": 1,
                              "column": 32,
                              "index": 32
                            },
                            "filename": null,
                            "identifierName": null
                          },
                          "typeAnnotation": {
                            "type": "TSNumberKeyword",
                            "start": 26,
                            "end": 32,
                            "loc": {
                              "start": {
                                "line": 1,
                                "column": 26,
                                "index": 26
                              },
                              "end": {
                                "line": 1,
                                "column": 32,
                                "index": 32
                              },
                              "filename": null,
                              "identifierName": null
                            }
                          }
                        }
                      }
                    ],
                    "body": {
                      "type": "BlockStatement",
                      "start": 35,
                      "end": 58,
                      "loc": {
                        "start": {
                          "line": 2,
                          "column": 0,
                          "index": 35
                        },
                        "end": {
                          "line": 4,
                          "column": 1,
                          "index": 58
                        },
                        "filename": null,
                        "identifierName": null
                      },
                      "body": [
                        {
                          "type": "ReturnStatement",
                          "start": 42,
                          "end": 55,
                          "loc": {
                            "start": {
                              "line": 3,
                              "column": 4,
                              "index": 42
                            },
                            "end": {
                              "line": 3,
                              "column": 17,
                              "index": 55
                            },
                            "filename": null,
                            "identifierName": null
                          },
                          "argument": {
                            "type": "BinaryExpression",
                            "start": 49,
                            "end": 54,
                            "loc": {
                              "start": {
                                "line": 3,
                                "column": 11,
                                "index": 49
                              },
                              "end": {
                                "line": 3,
                                "column": 16,
                                "index": 54
                              },
                              "filename": null,
                              "identifierName": null
                            },
                            "left": {
                              "type": "Identifier",
                              "start": 49,
                              "end": 50,
                              "loc": {
                                "start": {
                                  "line": 3,
                                  "column": 11,
                                  "index": 49
                                },
                                "end": {
                                  "line": 3,
                                  "column": 12,
                                  "index": 50
                                },
                                "filename": null,
                                "identifierName": "a"
                              },
                              "name": "a"
                            },
                            "operator": "+",
                            "right": {
                              "type": "Identifier",
                              "start": 53,
                              "end": 54,
                              "loc": {
                                "start": {
                                  "line": 3,
                                  "column": 15,
                                  "index": 53
                                },
                                "end": {
                                  "line": 3,
                                  "column": 16,
                                  "index": 54
                                },
                                "filename": null,
                                "identifierName": "b"
                              },
                              "name": "b"
                            }
                          }
                        }
                      ],
                      "directives": []
                    }
                  },
                  {
                    "type": "VariableDeclaration",
                    "start": 60,
                    "end": 81,
                    "loc": {
                      "start": {
                        "line": 5,
                        "column": 0,
                        "index": 60
                      },
                      "end": {
                        "line": 5,
                        "column": 21,
                        "index": 81
                      },
                      "filename": null,
                      "identifierName": null
                    },
                    "declarations": [
                      {
                        "type": "VariableDeclarator",
                        "start": 64,
                        "end": 80,
                        "loc": {
                          "start": {
                            "line": 5,
                            "column": 4,
                            "index": 64
                          },
                          "end": {
                            "line": 5,
                            "column": 20,
                            "index": 80
                          },
                          "filename": null,
                          "identifierName": null
                        },
                        "id": {
                          "type": "Identifier",
                          "start": 64,
                          "end": 65,
                          "loc": {
                            "start": {
                              "line": 5,
                              "column": 4,
                              "index": 64
                            },
                            "end": {
                              "line": 5,
                              "column": 5,
                              "index": 65
                            },
                            "filename": null,
                            "identifierName": "x"
                          },
                          "name": "x"
                        },
                        "init": {
                          "type": "CallExpression",
                          "start": 68,
                          "end": 80,
                          "loc": {
                            "start": {
                              "line": 5,
                              "column": 8,
                              "index": 68
                            },
                            "end": {
                              "line": 5,
                              "column": 20,
                              "index": 80
                            },
                            "filename": null,
                            "identifierName": null
                          },
                          "callee": {
                            "type": "Identifier",
                            "start": 68,
                            "end": 72,
                            "loc": {
                              "start": {
                                "line": 5,
                                "column": 8,
                                "index": 68
                              },
                              "end": {
                                "line": 5,
                                "column": 12,
                                "index": 72
                              },
                              "filename": null,
                              "identifierName": "add2"
                            },
                            "name": "add2"
                          },
                          "arguments": [
                            {
                              "type": "NumericLiteral",
                              "start": 73,
                              "end": 75,
                              "loc": {
                                "start": {
                                  "line": 5,
                                  "column": 13,
                                  "index": 73
                                },
                                "end": {
                                  "line": 5,
                                  "column": 15,
                                  "index": 75
                                },
                                "filename": null,
                                "identifierName": null
                              },
                              "extra": {
                                "rawValue": 11,
                                "raw": "11"
                              },
                              "value": 11
                            },
                            {
                              "type": "NumericLiteral",
                              "start": 77,
                              "end": 79,
                              "loc": {
                                "start": {
                                  "line": 5,
                                  "column": 17,
                                  "index": 77
                                },
                                "end": {
                                  "line": 5,
                                  "column": 19,
                                  "index": 79
                                },
                                "filename": null,
                                "identifierName": null
                              },
                              "extra": {
                                "rawValue": 22,
                                "raw": "22"
                              },
                              "value": 22
                            }
                          ]
                        }
                      }
                    ],
                    "kind": "var"
                  },
                  {
                    "type": "ExpressionStatement",
                    "start": 83,
                    "end": 98,
                    "loc": {
                      "start": {
                        "line": 6,
                        "column": 0,
                        "index": 83
                      },
                      "end": {
                        "line": 6,
                        "column": 15,
                        "index": 98
                      },
                      "filename": null,
                      "identifierName": null
                    },
                    "expression": {
                      "type": "CallExpression",
                      "start": 83,
                      "end": 97,
                      "loc": {
                        "start": {
                          "line": 6,
                          "column": 0,
                          "index": 83
                        },
                        "end": {
                          "line": 6,
                          "column": 14,
                          "index": 97
                        },
                        "filename": null,
                        "identifierName": null
                      },
                      "callee": {
                        "type": "MemberExpression",
                        "start": 83,
                        "end": 94,
                        "loc": {
                          "start": {
                            "line": 6,
                            "column": 0,
                            "index": 83
                          },
                          "end": {
                            "line": 6,
                            "column": 11,
                            "index": 94
                          },
                          "filename": null,
                          "identifierName": null
                        },
                        "object": {
                          "type": "Identifier",
                          "start": 83,
                          "end": 90,
                          "loc": {
                            "start": {
                              "line": 6,
                              "column": 0,
                              "index": 83
                            },
                            "end": {
                              "line": 6,
                              "column": 7,
                              "index": 90
                            },
                            "filename": null,
                            "identifierName": "console"
                          },
                          "name": "console"
                        },
                        "computed": false,
                        "property": {
                          "type": "Identifier",
                          "start": 91,
                          "end": 94,
                          "loc": {
                            "start": {
                              "line": 6,
                              "column": 8,
                              "index": 91
                            },
                            "end": {
                              "line": 6,
                              "column": 11,
                              "index": 94
                            },
                            "filename": null,
                            "identifierName": "log"
                          },
                          "name": "log"
                        }
                      },
                      "arguments": [
                        {
                          "type": "Identifier",
                          "start": 95,
                          "end": 96,
                          "loc": {
                            "start": {
                              "line": 6,
                              "column": 12,
                              "index": 95
                            },
                            "end": {
                              "line": 6,
                              "column": 13,
                              "index": 96
                            },
                            "filename": null,
                            "identifierName": "x"
                          },
                          "name": "x"
                        }
                      ]
                    }
                  },
                  {
                    "type": "ReturnStatement",
                    "start": 100,
                    "end": 109,
                    "loc": {
                      "start": {
                        "line": 7,
                        "column": 0,
                        "index": 100
                      },
                      "end": {
                        "line": 7,
                        "column": 9,
                        "index": 109
                      },
                      "filename": null,
                      "identifierName": null
                    },
                    "argument": {
                      "type": "Identifier",
                      "start": 107,
                      "end": 108,
                      "loc": {
                        "start": {
                          "line": 7,
                          "column": 7,
                          "index": 107
                        },
                        "end": {
                          "line": 7,
                          "column": 8,
                          "index": 108
                        },
                        "filename": null,
                        "identifierName": "x"
                      },
                      "name": "x"
                    }
                  }
                ],
                "directives": []
              },
              "comments": []
            }
            astJson: {
              "type": "File",
              "start": 0,
              "end": 109,
              "loc": {},
              "errors": [],
              "program": {},
              "comments": []
            }
            astJson: {
              "type": "File",
              "errors": [],
              "program": {
                "type": "Program",
                "start": 0,
                "end": 109,
                "loc": {},
                "sourceType": "script",
                "interpreter": null,
                "body": [],
                "directives": []
              },
              "comments": []
            }
            
            """;
    }
}
